<?php

namespace App\Http\Middleware;

use Illuminate\Foundation\Http\Middleware\VerifyCsrfToken as Middleware;
use Symfony\Component\HttpFoundation\Response;

class VerifyCsrfToken extends Middleware
{
    /**
     * The URIs that should be excluded from CSRF verification.
     *
     * @var array<int, string>
     */
    protected $except = [
        //
    ];

    protected function addCookieToResponse($request, $response): Response
    {
        $response->headers->setCookie(
            cookie(
                'XSRF-TOKEN',
                $request->session()->token(),
                60, // Masa berlaku cookie (menit)
                '/', // Path
                null, // Domain
                true, // Secure
                true, // HttpOnly
                false, // Raw
                'Strict' // SameSite
            )
        );

        return $response;
    }
}
