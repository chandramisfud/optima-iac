<?php

namespace App\Logging;

use Monolog\Handler\RotatingFileHandler;
use Log;

class RotateLog
{
    /**
     * Customize the given logger instance.
     *
     * @param  \Illuminate\Log\Logger  $logger
     * @return void
     */
    public function __invoke($logger)
    {
        foreach ($logger->getHandlers() as $handler) {
            if ($handler instanceof RotatingFileHandler) {
                $hour = date('H');
                $handler->setFilenameFormat("{filename}-{date}-".$hour, 'Y-m-d');
            }
        }
    }
}
