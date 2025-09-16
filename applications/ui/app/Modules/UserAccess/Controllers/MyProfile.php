<?php

namespace App\Modules\UserAccess\Controllers;

use Illuminate\Support\Facades\Storage;
use Intervention\Image\Facades\Image;
use Session;
use App\Http\Requests;
use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

use Wording;

class MyProfile extends Controller
{
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function myProfilePage()
    {
        $title = 'User';
        return view('UserAccess::my-profile.index', compact('title'));
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
            Log::info($api);
            try {
                $path = '/assets/media/users/';
                if (!Storage::disk('optima')->exists($path)) {
                    Storage::disk('optima')->makeDirectory($path);
                }

                $canvas = Image::canvas('400', '400');
                list($width, $height) = getimagesize($file);
                if ($width <= $height) {
                    $resizeImage  = Image::make($file)->resize('400', null, function($constraint) {
                        $constraint->aspectRatio();
                    });
                } else {
                    $resizeImage  = Image::make($file)->resize(null, '400', function($constraint) {
                        $constraint->aspectRatio();
                    });
                }
                $canvas->insert($resizeImage, 'center');
                $canvas->save(public_path($path . $request->session()->get('userid') . '.png'));

                return array(
                    'error'     => false,
                    'message'   => 'Profile picture has changed',
                    'redirect'  => $request->x
                );
            } catch (\Exception $e) {
                Log::error($e->getMessage());
                if (stripos($e->getMessage(), 'connection')) {
                    return array(
                        'error' => true,
                        'data' => [],
                        'message' => \App\Helpers\Wording::timeout()
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

    public function changeOldPassword(Request $request)
    {
        if (isset($request->isLogin)) {
            $this->clearSession($request);
            return [
                'islogin' => 1
            ];
        } else {
            $api = config('app.api') . '/auth/changeoldpassword';
            $data = [
                'password'         => $request->new_password
            ];
            Log::info($data);
            try {
                $response = Http::timeout(180)->withToken($this->token)->post($api, $data);
                if ($response->status() === 200) {
                    return array(
                        'error'     => false,
                        'message'   => json_decode($response)->message,
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
                Log::error((string)$err);
                return $err;
            }
        }
    }

}
