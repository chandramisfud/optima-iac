<?php
namespace App\Helpers;

use Exception;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class CallApi
{
    protected string $address;
    protected mixed $token;

    public function __construct()
    {
        $this->address = config('app.api');
    }

    public function getUsingToken ($token, $uri, $query = null): bool|string
    {
        try {
            $api = $this->address . $uri;
            Log::info('GET ' . $api);
            if ($query) Log::info(json_encode($query));

//            debugging
//            return '';
            $response = Http::timeout(180)->withToken($token)->get($api, $query);
            if ($response->status() === 200) {
                $resVal = json_decode($response)->values;
                $message = json_decode($response)->message;
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => false,
                    'data'      => $resVal,
                    'message'   => $message
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning($response->status());
                Log::warning($response->reason());
                Log::warning($message);
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'status'    => 500,
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            ]);
        }
    }

    public function postUsingToken ($token, $uri, $body): bool|string
    {
        try {
            $api = $this->address . $uri;
            Log::info('POST ' . $api);
            if ($body) Log::info(json_encode($body));
//            debugging
//            return '';
            $response = Http::timeout(180)->withToken($token)->post($api, $body);
            if ($response->status() === 200) {
                $resVal = (isset(json_decode($response)->data) ?? null);
                $message = json_decode($response)->message;
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => false,
                    'data'      => $resVal,
                    'message'   => $message
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning($response->status());
                Log::warning($response->reason());
                Log::warning($message);
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'status'    => 500,
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            ]);
        }
    }

    public function postMultipartUsingToken($token, $uri, $body, $file = null, $contentName = ""): bool|string
    {
        try {
            $api = $this->address . $uri;
            Log::info('POST ' . $api);
            if ($body) Log::info(json_encode($body));
//            debugging
//            return '';
            if ($file) {
                $response = Http::timeout(180)->withToken($token)->attach($contentName, $file->getContent(), $file->getClientOriginalName())->post($api, [
                    'name'      => 'data',
                    'contents'  => json_encode($body)
                ]);
            } else {
                $response = Http::timeout(180)->withToken($token)->post($api, [
                    'name'      => 'data',
                    'contents'  => json_encode($body)
                ]);
            }
            if ($response->status() === 200) {
                $resVal = (isset(json_decode($response)->data) ?? null);
                $message = json_decode($response)->message;
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => false,
                    'data'      => $resVal,
                    'message'   => $message
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning($response->status());
                Log::warning($response->reason());
                Log::warning($message);
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'status'    => 500,
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            ]);
        }
    }

    public function putUsingToken ($token, $uri, $body): bool|string
    {
        try {
            $api = $this->address . $uri;
            Log::info('PUT ' . $api);
            if ($body) Log::info(json_encode($body));
//            debugging
//            return '';
            $response = Http::timeout(180)->withToken($token)->put($api, $body);
            if ($response->status() === 200) {
                $resVal = (isset(json_decode($response)->data) ?? null);
                $message = json_decode($response)->message;
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => false,
                    'data'      => $resVal,
                    'message'   => $message
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning($response->status());
                Log::warning($response->reason());
                Log::warning($message);
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'status'    => 500,
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            ]);
        }
    }

    public function putMultipartUsingToken ($token, $uri, $body, $file = null, $contentName = ""): bool|string
    {
        try {
            $api = $this->address . $uri;
            Log::info('PUT ' . $api);
            if ($body) Log::info(json_encode($body));
//            debugging
//            return '';
            if ($file) {
                $response = Http::timeout(180)->withToken($token)->attach($contentName, $file->getContent(), $file->getClientOriginalName())->put($api, [
                    'name'      => 'data',
                    'contents'  => json_encode($body)
                ]);
            } else {
                $response = Http::timeout(180)->withToken($token)->put($api, [
                    'name'      => 'data',
                    'contents'  => json_encode($body)
                ]);
            }
            if ($response->status() === 200) {
                $resVal = (isset(json_decode($response)->data) ?? null);
                $message = json_decode($response)->message;
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => false,
                    'data'      => $resVal,
                    'message'   => $message
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning($response->status());
                Log::warning($response->reason());
                Log::warning($message);
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'status'    => 500,
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            ]);
        }
    }

    public function deleteUsingToken ($token, $uri, $body): bool|string
    {
        try {
            $api = $this->address . $uri;
            Log::info('DELETE ' . $api);
            if ($body) Log::info(json_encode($body));
//            debugging
//            return '';
            $response = Http::timeout(180)->withToken($token)->delete($api, $body);
            if ($response->status() === 200) {
                $resVal = (isset(json_decode($response)->data) ?? null);
                $message = json_decode($response)->message;
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => false,
                    'data'      => $resVal,
                    'message'   => $message
                ]);
            } else {
                $message = json_decode($response)->message;
                Log::warning($response->status());
                Log::warning($response->reason());
                Log::warning($message);
                return json_encode([
                    'status'    => $response->status(),
                    'error'     => true,
                    'data'      => [],
                    'message'   => $message
                ]);
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return json_encode([
                'status'    => 500,
                'error'     => true,
                'data'      => [],
                'message'   => $e->getMessage()
            ]);
        }
    }
}
