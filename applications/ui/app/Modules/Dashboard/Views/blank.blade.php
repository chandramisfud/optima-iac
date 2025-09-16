@extends('layouts/layoutMaster')

@section('title', 'Dashboard')

@section('vendor-style')

@endsection

@section('page-style')

@endsection

@section('button-toolbar')

@endsection

@section('content')


@endsection

@section('vendor-script')

@endsection

@section('page-script')
<script>
    let expiredChangeStr = "{{ @Session::get('password_change') }}";
    let expiredChange = new Date(expiredChangeStr).getTime();
    let now = new Date().getTime();
    if (Math.floor((now-expiredChange)/(24*3600*1000)) >= 50) {
        let expiredInterval = 60 - Math.floor((now-expiredChange)/(24*3600*1000));
        swal.fire({
            icon: "warning",
            title: 'Warning',
            text: "Your password expired in " + expiredInterval + " days, please change your password",
            allowOutsideClick: false,
            allowEscapeKey: false,
        });
    }
</script>
@endsection
