<?php
namespace App\Helpers;

class Formatted
{
    function getNameFromNumber($num): string
    {
        $numeric = ($num - 1) % 26;
        $letter = chr(65 + $numeric);
        $num2 = intval(($num - 1) / 26);
        if ($num2 > 0) {
            return $this->getNameFromNumber($num2) . $letter;
        } else {
            return $letter;
        }
    }

    function isNotDate($date): bool
    {
        return strtotime($date) === false;
    }
}
