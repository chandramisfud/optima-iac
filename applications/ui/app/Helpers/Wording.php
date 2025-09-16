<?php

namespace App\Helpers;

use Illuminate\Support\Facades\Log;

class Wording
{
    public static function timeout(): string
    {
        return "Cannot access server, check your connection";
    }

    public static function login(): string
    {
        return "Login failed";
    }
}
