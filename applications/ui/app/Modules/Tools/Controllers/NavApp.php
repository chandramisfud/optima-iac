<?php

namespace App\Modules\Tools\Controllers;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Log;

class  NavApp extends Controller
{
    protected mixed $token;

    protected string $method = 'aes-128-ctr'; // default cipher method if none supplied
    protected string $AES_METHOD = 'aes-256-cbc'; // default cipher method if none supplied
    protected string $KEY_APP = 'Re0IgCaFLMoVWfesL4jnfPK3ox6NhLpF';

    public function __construct()
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    protected function iv_bytes1()
    {
        return openssl_cipher_iv_length($this->AES_METHOD);
    }

    public function NAVAppPage(Request $request){
        $title = "NAV Access";
        $subtitle = "Connect to NAV App";

        $email = $request->session()->get('email');
        $dtcrypt = date('Y-m-d H:i');
        $encrypt = $this->NAVGenerate($dtcrypt . '/' . $email);

        Log::info($email);
        Log::info($dtcrypt);
        $urldata = $encrypt['link'];
        Log::info($urldata);

        return view('Tools::nav-app.index', compact('title', 'subtitle', 'urldata'));
    }

    public function NAVGenerate($data){
        $iv = openssl_random_pseudo_bytes($this->iv_bytes1());
        $encrypt = openssl_encrypt($data, $this->AES_METHOD, $this->KEY_APP, OPENSSL_RAW_DATA, $iv);

        $link = config('app.nav_web') . '/login?token=' . bin2hex($encrypt) . '&iv=' . bin2hex($iv);

        return array(
            'iv'    => bin2hex($iv),
            'token' => bin2hex($encrypt),
            'link'  => $link
        );
    }

}
