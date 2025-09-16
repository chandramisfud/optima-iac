<?php

namespace App\Helpers;

use Illuminate\Support\Facades\Log;

class MyEncrypt
{
    protected string $method = 'AES-256-CBC'; // default cipher method if none supplied
    protected string $key = "";

    protected static function iv_bytes(): bool|int
    {
        $encryptClass = new MyEncrypt();
        return openssl_cipher_iv_length($encryptClass->method);
    }

    public static function encrypt($data): string
    {
        $encryptClass = new MyEncrypt();
        $iv = openssl_random_pseudo_bytes(MyEncrypt::iv_bytes());
        return bin2hex($iv) . openssl_encrypt($data, $encryptClass->method, $encryptClass->key, 0, $iv);
    }

    public static function decrypt($data): bool|string
    {
        $encryptClass = new MyEncrypt();
        $iv_str_len = 2  * MyEncrypt::iv_bytes();
        if(preg_match("/^(.{" . $iv_str_len . "})(.+)$/", $data, $regs)) {
          list(, $iv, $crypted_string) = $regs;
          if(ctype_xdigit($iv) && strlen($iv) % 2 == 0) {
            return openssl_decrypt($crypted_string, $encryptClass->method, $encryptClass->key, 0, hex2bin($iv));
          }
        }

        $text = $encryptClass->decryptCSharpAES256CBC($data, '0123456789abcdef0123456789abcdef', 'abcdef0123456789');

        if ($text) return $text;

        return FALSE; // failed to decrypt
    }

    public static function decryptCSharpAES256CBC($encrypted, $key, $iv)
    {
        $base64 = str_replace(['-', '_'], ['+', '/'], $encrypted);
        $padding = strlen($base64) % 4;
        if ($padding > 0) {
            $base64 .= str_repeat('=', 4 - $padding); // Menambahkan padding jika diperlukan
        }

        // Decode Base64
        $encryptedBytes = base64_decode($base64);

        // Decrypt using AES-256-CBC
        $decrypted = openssl_decrypt($encryptedBytes, 'aes-256-cbc', $key, OPENSSL_RAW_DATA, $iv);

        return $decrypted;
    }


}
