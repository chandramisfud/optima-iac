<!DOCTYPE html>
<html lang="en">
<head><base href="">
    <title>Optima SN - @yield('title')</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta charset="utf-8" />
    <meta property="og:locale" content="en_US" />
    <meta property="og:type" content="article" />
    <meta property="og:title" content="Optima SN - PT. Danone" />
    <meta property="og:url" content="https://xvautomation.com" />
    <meta property="og:site_name" content="Optima SN" />
    <meta name="csrf-token" content="{{ csrf_token() }}">
    <meta http-equiv="Content-Security-Policy" content="
        default-src 'self';
        worker-src data: 'unsafe-eval' 'unsafe-inline' blob:;
        img-src * 'self' data: https:;
        base-uri 'self';
        object-src 'none';
        script-src 'self' 'unsafe-inline';
        style-src 'self' 'unsafe-inline' https://fonts.googleapis.com;
        font-src 'self' https://fonts.gstatic.com;"
    />

    <link rel="shortcut icon" href="{{ asset('assets/media/logos/logo.ico') }}" />
    <!--begin::Fonts-->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700" />
    <!--end::Fonts-->

    @include('panels.style')
</head>
<script>
    const file_host = '{{ config('app.url') }}';
</script>

@extends('layouts.contentLayoutMaster')
