<?php

namespace App\Modules\Auth\Controllers;

use App\Helpers\Wording;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Contracts\Encryption\DecryptException;
use Illuminate\Support\Facades\Crypt;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Illuminate\Contracts\View\View;
use Illuminate\Http\Client\Response;
use Illuminate\Routing\Redirector;
use Illuminate\Http\RedirectResponse;

class AuthController extends Controller
{
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            if ($request->session()->get('token')) {
                $this->token = $request->session()->get('token');
            }

            return $next($request);
        });
    }

    public function loginPage(): View
    {
        return view('Auth::login');
    }

    public function verifyCaptcha(Request $request): array|Response
    {
        $api = 'https://www.google.com/recaptcha/api/siteverify';
        $data = [
            'secret'    => config('app.captcha_key'),
            'response'  => $request->token
        ];
        Log::info($api);
        Log::info(json_encode($data));
        try {
            $response = Http::asForm()->post($api, $data);
            if ($response->status() === 200) {
                return $response;
            } else {
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => "Not Verified, you detected are a robot!"
                );
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            if (stripos($e->getMessage(), 'connection')) {
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => Wording::timeout()
                );
            } else {
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => Wording::login()
                );
            }
        }
    }

    public function signIn(Request $request): array
    {
        $api = config('app.api') . '/auth/login';
        $data = [
            'id'        => $request->email,
            'password'  => $request->password
        ];
        Log::info($api);
        Log::info(json_encode(['id' => $request->email]));
        try {
            $response = Http::post($api, $data);
            if ($response->status() === 200) {
                $resValue = json_decode($response)->values;
                $request->session()->put('userid', $resValue->id);
                $request->session()->put('name', $resValue->userName);
                $request->session()->put('email', $resValue->email);
                $request->session()->put('contact_info', $resValue->contactInfo);
                $request->session()->put('token', $resValue->token);
                $request->session()->put('profile_login', $resValue->profile);
                $request->session()->put('profile_picture', $resValue->profilePictureUrl);
                $request->session()->put('login_freeze_time', $resValue->loginFreezeTime);
                $request->session()->put('password_change', $resValue->password_Change);

                return array(
                    'error'     => false,
                    'data'      => $resValue,
                );
            } else {
                $resValue = json_decode($response)->values;

                $resData['failedCount']         = $resValue->loginFailedCount;
                $resData['login_freeze_time']   = $resValue->loginFreezeTime;
                $resData['err_code']            = $resValue->errCode;

                $failed3Times = str_contains($resValue->errMessage, 'You have failed to login 3 times');
                $loginFreezeTime = 15 - $resValue->loginFreezeTime;
                if($failed3Times==1) {
                    $resData['err_message'] = $resValue->errMessage . ' after ' . $loginFreezeTime . ' minutes';
                }else{
                    $resData['err_message'] = $resValue->errMessage;
                }
                Log::warning(json_encode($resValue));
                return array(
                    'error' => true,
                    'data' => $resData,
                    'message' => $resData['err_message']
                );
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            if (stripos($e->getMessage(), 'connection')) {
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => Wording::timeout()
                );
            } else {
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => Wording::login()
                );
            }
        }
    }

    public function clearSession(Request $request)
    {
        $request->session()->flush();
    }

    public function signOut(Request $request): array|Redirector|RedirectResponse
    {
        $api = config('app.api') . '/auth/resetislogin';
        Log::info($api);
        try {
            Http::withToken($request->session()->get('token'))->put($api);
        } catch (\Exception $e) {
            Log::error($e->getMessage());
        }
        $request->session()->flush();
        return redirect('/login-page');
    }

    public function forgotPasswordPage(Request $request)
    {
        try {
            $decrypt = Crypt::decryptString(json_encode($request->i));
            $email = substr($decrypt, 0, strlen($decrypt)-19);
            $sDate1 = substr($decrypt, -19);
            $sDate2 = date('Y-m-d H:m:s');
            $nInterval = strtotime($sDate2) - strtotime($sDate1);
            Log::info('Link forgot ' . $email . ' Interval '. $nInterval/60);
            return view('Auth::forgot-password', compact('email'));
        } catch (DecryptException $e) {
            Log::error($e);
        }
    }

    public function forgotPassword(Request $request): array
    {
        $cekEmailUser = $this->cekEmailUser(urldecode($request->email));
        if (!json_decode($cekEmailUser)->error) {
            if(json_decode($cekEmailUser)->data->isdeleted===0) { // cek user active or inactive
                if(json_decode($cekEmailUser)->data->expLastLogin<=45) { // cek last login user expired
                    $mail_encrypted = Crypt::encryptString($request->email . ' ' . date('Y-m-d H:m:s'));

                    $api = config('app.api') . '/auth/sendemail';
                    $data = [
                        'email' => $request->email,
                        'subject' => "Optima no-reply [forgot password]",
                        'body' => "This email was sent automatically by Optima System in responses to reset password request. This is the next step in the password recovery process.<br><p>To reset your password and access your account, either click on the button or copy and paste the follow link<p><a href='" . config('app.url') . "/auth/forgot-password-page?i=" . $mail_encrypted . "'>" . config('app.url') . "/auth/forgot-password-page?i=" . $mail_encrypted . "</a><br><br><p>Thank you,<br>Optima System"
                    ];
                    Log::info($api);
                    Log::info(json_encode($data));
                    try {
                        $response = Http::asForm()->post($api, $data);
                        if ($response->status() === 200) {
                            return array(
                                'error' => false,
                                'data' => json_decode($response),
                                'message' => 'Password reset link send your email, Please check your email',
                            );
                        } else {
                            $message = json_decode($response)->message;
                            Log::warning($message);
                            return array(
                                'error' => true,
                                'data' => [],
                                'message' => $message
                            );
                        }
                    } catch (\Exception $e) {
                        Log::error($e->getMessage());
                        if (stripos($e->getMessage(), 'connection')) {
                            return array(
                                'error' => true,
                                'data' => [],
                                'message' => Wording::timeout()
                            );
                        } else {
                            return array(
                                'error' => true,
                                'data' => [],
                                'message' => 'Reset Password Failed'
                            );
                        }
                    }
                } else {
                    // Run deactivate user
                    $api = config('app.api') . '/auth/user/email';
                    $data = [
                        'email' => urldecode($request->email)
                    ];
                    Log::info($api);
                    Log::info(json_encode($data));
                    try {
                        $response = Http::delete($api, $data);
                        if ($response->status() === 200) {
                            return array(
                                'error' => true,
                                'data' => [],
                                'message' => "User is deactivated due to inactivity for more than 45 days, Please contact your the Administrator for reactivation"
                            );
                        } else {
                            $message = json_decode($response)->message;
                            Log::warning($message);
                            return array(
                                'error' => true,
                                'data' => [],
                                'message' => $message
                            );
                        }
                    } catch (\Exception $e) {
                        Log::error($e->getMessage());
                        if (stripos($e->getMessage(), 'connection')) {
                            return array(
                                'error' => true,
                                'data' => [],
                                'message' => Wording::timeout()
                            );
                        } else {
                            return array(
                                'error' => true,
                                'data' => [],
                                'message' => 'Forgot Password Failed'
                            );
                        }
                    }
                }
            } else {
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => "Your email address is inactive"
                );
            }
        } else {
            return array(
                'error' => true,
                'data' => [],
                'message' => json_decode($cekEmailUser)->message
            );
        }
    }

    public function changePassword(Request $request): array
    {
        $decrypt = Crypt::decryptString(json_encode($request->i));
        $email = substr($decrypt, 0, strlen($decrypt)-19);

        $data = [
            'email' => $email,
            'password' => $request->new_password,
        ];
        $api = config('app.api') . '/auth/changepassword';
        Log::info($api);
        Log::info(json_encode($data));
        try {
            $response = Http::post($api, $data);
            if ($response->status() === 200) {
                $code = json_decode($response)->code;
                if ($code === 200)
                    return array(
                        'error' => false,
                        'message' => 'Password changed successfully'
                    );
            } else {
                $message = json_decode($response)->message;
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            if (stripos($e->getMessage(), 'connection')) {
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => Wording::timeout()
                );
            } else {
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => Wording::login()
                );
            }
        }
    }

    public function changePasswordNewUser(Request $request): array
    {
        $data = [
            'email'     => $request['email'],
            'password'  => $request['new_password'],
        ];
        $api = config('app.api') . '/auth/changepassword';
        Log::info($api);
        Log::info(json_encode($data));
        try {
            $response = Http::post($api, $data);
            if ($response->status() === 200) {
                $code = json_decode($response)->code;
                if ($code === 200)
                    return array(
                        'error' => false,
                        'message' => 'Password changed successfully'
                    );
            } else {
                $message = json_decode($response)->message;
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            if (stripos($e->getMessage(), 'connection')) {
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => Wording::timeout()
                );
            } else {
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => Wording::login()
                );
            }
        }
    }

    protected function cekEmailUser($email): bool|string
    {
        $data = [
            'email'         => $email,
        ];
        $api = config('app.api') . '/auth/user/email';
        Log::info($api);
        Log::info(json_encode($data));
        try {
            $response = Http::get($api, $data);
            if ($response->status() === 200) {
                $code = json_decode($response)->code;
                $message = json_decode($response)->message;
                if ($code === 200)
                {
                    return json_encode([
                        'error'     => false,
                        'data'      => json_decode($response)->values,
                        'message'   => "get data success"
                    ]);
                } else {
                    return json_encode([
                        'error'     => true,
                        'data'      => [],
                        'message'   => $message
                    ]);
                }
            } else {
                $message = json_decode($response)->message;
                Log::warning($message);
                return json_encode([
                    'error'     => true,
                    'data'      => [],
                    'message'   => "error : Send Email Failed"
                ]);
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            if (stripos($e->getMessage(), 'connection')) {
                return json_encode([
                    'error'     => true,
                    'data'      => [],
                    'message'   => Wording::timeout()
                ]);
            } else {
                return json_encode([
                    'error'     => true,
                    'data'      => [],
                    'message'   => Wording::timeout()
                ]);
            }
        }

    }

    public function getProfileLogin(Request $request): array
    {
        $api = config('app.api'). '/auth/profile';
        Log::info('Get API ' . $api);
        try {
            $response = Http::withToken($this->token)->get($api);
            if ($response->status() === 200) {
                return array(
                    'error'     => false,
                    'data'      => json_decode($response)->values
                );
            } else {
                $message = json_decode($response)->message;
                return array(
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                );
            }
        } catch (\Exception $e) {
            Log::error('get API ' . $api);
            Log::error($e->getMessage());
            return array(
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            );
        }
    }

    public function isLoginPut(Request $request): array
    {
        $api = config('app.api') . '/auth/islogin';
        Log::info($api);
        try {
            $response = Http::withToken($this->token)->put($api);
            if ($response->status() === 200) {
                $resValue = json_decode($response)->values;
                $message = json_decode($response)->message;
                $request->session()->put('islogin', $resValue);
                return array(
                    'error'     => false,
                    'message'   => $message,
                );
            } else {
                $message = json_decode($response)->message;
                Log::warning($message);
                return array(
                    'error' => true,
                    'data' => [],
                    'message' => $message
                );
            }
        } catch (\Exception $e) {
            Log::error($e->getMessage());
            if (stripos($e->getMessage(), 'connection')) {
                $err = [
                    'error' => true,
                    'data' => [],
                    'message' => Wording::timeout()
                ];
            } else {
                $err = [
                    'error' => true,
                    'data' => [],
                    'message' => Wording::login()
                ];
            }
            Log::error(json_encode($err));
            return $err;
        }
    }

    public function profilePushSession(Request $request): array
    {
        if (isset($request->isLogin)) {
            $this->clearSession($request);
            return [
                'islogin' => 1
            ];
        } else {
            $islogin = $request->session()->get('islogin');
            $userid = $request->session()->get('userid');
            $name = $request->session()->get('name');
            $email = $request->session()->get('email');
            $contact_info = $request->session()->get('contact_info');
            $login_freeze_time = $request->session()->get('login_freeze_time');
            $password_change = $request->session()->get('password_change');


            $api = config('app.api') . '/auth/token/changeprofile';
            $data = [
                'profileID'         => $request->profileId
            ];
            Log::info('POST Change Profile: ' . $api);
            Log::info(json_encode($data));
            Log::info('Before: ' . $request->session()->get('email') . ' - ' . $request->session()->get('profile'));
            try {
                $response = Http::withToken($this->token)->asForm()->post($api, $data);
                if ($response->status() === 200) {
                    $request->session()->flush();
                    $resValue = json_decode($response)->values;

                    $request->session()->put('islogin', $islogin);
                    $request->session()->put('userid', $userid);
                    $request->session()->put('name', $name);
                    $request->session()->put('email', $email);
                    $request->session()->put('contact_info', $contact_info);
                    $request->session()->put('login_freeze_time', $login_freeze_time);
                    $request->session()->put('password_change', $password_change);
                    $request->session()->put('role', $resValue->userGroupID);
                    $request->session()->put('userlevel', $resValue->userLevel);
                    $request->session()->put('rolename', $resValue->userGroupName);
                    $request->session()->put('profile', $resValue->userProfileId);
                    $request->session()->put('token', $resValue->token);
                    $request->session()->put('profileCategories', $resValue->profileCategories);

                    Log::info('After: ' . $request->session()->get('email') . ' - ' . $request->session()->get('profile'));
                    return array(
                        'error'     => false,
                        'message'   => 'Select Profile Success',
                    );
                } else {
                    $message = json_decode($response)->message;
                    Log::warning($message);
                    return array(
                        'error' => true,
                        'data' => [],
                        'message' => $message
                    );
                }
            } catch (\Exception $e) {
                Log::error($e->getMessage());
                if (stripos($e->getMessage(), 'connection')) {
                    $err = [
                        'error' => true,
                        'data' => [],
                        'message' => Wording::timeout()
                    ];
                } else {
                    $err = [
                        'error' => true,
                        'data' => [],
                        'message' => Wording::login()
                    ];
                }
                Log::error(json_encode($err));
                return $err;
            }
        }
    }

    public function myProfilePictureStore(Request $request): array
    {
        if (isset($request->isLogin)) {
            $this->clearSession($request);
            return [
                'islogin' => 1
            ];
        } else {
            $api = config('app.api') . '/auth/picture';
            $file = $request->file('avatar');
            $fileName =  $file->getClientOriginalExtension();
            Log::info($api);
            try {
                $response = Http::withToken($this->token)->attach(
                    'formFile', file_get_contents($request->avatar), $fileName
                )->post($api);
                if ($response->status() === 200) {
                    $resValue = json_decode($response)->values;
                    $request->session()->put('profile_picture', $resValue);
                    return array(
                        'error'     => false,
                        'message'   => 'Profile picture changed success fully',
                        'redirect'  => $request->x
                    );
                } else {
                    $message = json_decode($response)->message;
                    Log::warning($message);
                    return array(
                        'error' => true,
                        'data' => [],
                        'message' => $message
                    );
                }
            } catch (\Exception $e) {
                Log::error($e->getMessage());
                if (stripos($e->getMessage(), 'connection')) {
                    return array(
                        'error' => true,
                        'data' => [],
                        'message' => Wording::timeout()
                    );
                } else {
                    return array(
                        'error' => true,
                        'data' => [],
                        'message' => Wording::login()
                    );
                }
            }
        }
    }

}
