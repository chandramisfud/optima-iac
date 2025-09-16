<?php

namespace App\Modules\Tools\Controllers;

use App\Http\Controllers\Controller;
use Exception;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class  Budget extends Controller
{
    protected mixed $token;

    public function __construct(Request $request)
    {
        $this->middleware(function ($request, $next) {
            $this->token = $request->session()->get('token');

            return $next($request);
        });
    }

    public function uploadPage()
    {
        $title = "Budget";
        return view('Tools::budget.index', compact('title'));
    }

    public function uploadXls(Request $request) {
        $api = config('app.api') . '/tools/upload/budget';
        Log::info('post API ' . $api);
        try {
            if ($request->file('file')) {
                $categories = $request->session()->get('profileCategories');
                $arrProfileCategories = array();
                foreach ($categories as $category) {
                    array_push($arrProfileCategories, $category->categoryLongDesc);
                }
                $last  = array_slice($arrProfileCategories, -1);
                $first = join(', ', array_slice($arrProfileCategories, 0, -1));
                $profileCategories  = join(' and ', array_filter(array_merge(array($first), $last), 'strlen'));
                $wordingProfile = ((count($arrProfileCategories) > 1) ? "profile categories are " . $profileCategories  : "profile category is $profileCategories");
                if (!$this->findValueObject($categories, 'RC') && !$this->findValueObject($categories, 'TS')) {
                    return array(
                        'error' => true,
                        'message' => "Disallowed to upload this template, " . $wordingProfile
                    );
                }
                $response = Http::timeout(180)->withToken($this->token)
                    ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                    ->post($api, [
                        'name'      => 'formFile',
                        'contents'  => $request->file('file')->getContent()
                    ]);
                if ($response->status() === 200) {
                    return json_encode([
                        'data'      => json_decode($response)->values,
                        'error'     => false,
                        'code'      => json_decode($response)->code,
                        'message'   => json_decode($response)->message,
                    ]);
                } else {
                    return array(
                        'error' => true,
                        'code'      => json_decode($response)->code,
                        'message'   => json_decode($response)->message,
                    );
                }
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'message' => "Upload failed"
            );
        }
    }

    public function uploadXlsDC(Request $request) {
        $api = config('app.api') . '/tools/upload/budgetdc';
        Log::info('post API ' . $api);
        try {
            if ($request->file('file')) {
                $categories = $request->session()->get('profileCategories');
                $arrProfileCategories = array();
                foreach ($categories as $category) {
                    array_push($arrProfileCategories, $category->categoryLongDesc);
                }
                $last  = array_slice($arrProfileCategories, -1);
                $first = join(', ', array_slice($arrProfileCategories, 0, -1));
                $profileCategories  = join(' and ', array_filter(array_merge(array($first), $last), 'strlen'));
                $wordingProfile = ((count($arrProfileCategories) > 1) ? "profile categories are " . $profileCategories  : "profile category is $profileCategories");
                if (!$this->findValueObject($categories, 'DC')) {
                    return array(
                        'error' => true,
                        'message' => "Disallowed to upload this template, " . $wordingProfile
                    );
                }
                $response = Http::timeout(180)->withToken($this->token)
                    ->attach('formFile', $request->file('file')->getContent(), $request->file('file')->getClientOriginalName())
                    ->post($api, [
                        'name'      => 'formFile',
                        'contents'  => $request->file('file')->getContent()
                    ]);

                if ($response->status() === 200) {
                    return json_encode([
                        'data'      => json_decode($response)->values,
                        'error'     => false,
                        'code'      => json_decode($response)->code,
                        'message'   => json_decode($response)->message,
                    ]);
                } elseif ($response->status() === 409) {
                    return json_encode([
                        'data'      => (json_decode($response)->values ?? []),
                        'error'     => false,
                        'code'      => json_decode($response)->code,
                        'message'   => json_decode($response)->message,
                    ]);
                } else {
                    return array(
                        'error' => true,
                        'code'      => $response->status(),
                        'message'   => json_decode($response)->message,
                    );
                }
            }
        } catch (Exception $e) {
            Log::error($e->getMessage());
            return array(
                'error' => true,
                'message' => "Upload failed"
            );
        }
    }

    public function findValueObject($array, $value): bool
    {

        foreach ( $array as $element ) {
            if ( $value == $element->categoryShortDesc ) {
                return true;
            }
        }

        return false;
    }
}
