<!-- @extends('errors::minimal')
@section('title', __('Service Unavailable'))
@section('code', '503')
@section('message', __($exception->getMessage() ?: 'Service Unavailable')) -->
<html>
<head>
<style>
img{
    width:100%;
    height:100%;
}
</style>
</head>

<body>
<img src="{{asset('/assets/media/bg/mainten.png')}}" alt="">
</body>
</html>
