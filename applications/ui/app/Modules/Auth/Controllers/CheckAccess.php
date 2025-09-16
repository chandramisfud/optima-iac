<?php

namespace App\Modules\Auth\Controllers;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Log;

class CheckAccess extends Controller
{
    function checkAccess(Request $request)
    {
        try {
            $list_access = $request->session()->get('user_access');
            $menu_exist = $this->findObjectById($list_access, $request->menuid);
            if ($menu_exist) {
                foreach ($list_access as $list) {
                    if ($list->menuid == $request->menuid) {
                        $access_name = $request->access_name;
                        if ($list->$access_name) {
                            return json_encode([
                                "error"     => false
                            ]);
                        } else {
                            return json_encode([
                                'error'     => true,
                                'message'   => "Feature is not allowed for this user"
                            ]);
                        }
                    }
                }
            } else {
                return json_encode([
                    'error'     => true,
                    'message'   => "Feature is not allowed for this user"
                ]);
            }
        } catch (\Exception $e) {
            Log::error('Auth->checkAccess ' . $e->getMessage());
            return $e->getMessage();
        }
    }

    public function checkSession(Request $request): bool
    {
        return $request->session()->has('userid');
    }

    private function findObjectById ($array, $id){

        foreach ( $array as $element ) {
            if ( $id == $element->menuid ) {
                return true;
            }
        }

        return false;
    }

}
