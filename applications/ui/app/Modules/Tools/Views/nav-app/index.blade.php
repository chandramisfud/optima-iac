@extends('layouts/layoutMaster')

@section('title', @$title)

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method">Connect to NAV App</small>
    </span>
@endsection

@section('button-toolbar-left')

@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')

@endsection


@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script>
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            }
        });

        let urldata = "{{ @$urldata }}";
        let userid = "{{ @Session::get('user')['userid'] }}";
        let urllink = urldata.replace(/&amp;/g, '&');

        $(document).ready(function () {
                Swal.fire({
                title: 'Make sure the vpn connection is activated',
                icon: "info",
                showCancelButton: true,
                cancelButtonColor: '#AAAAAA',
                confirmButtonText: "Proceed",
                allowOutsideClick: false,
                allowEscapeKey: false,
                cancelButtonText: 'Cancel',
            }).then(function (result) {
                if (result.isConfirmed) {
                    window.location.href = urllink;
                }else{
                    window.location.href = '/';
                }
            });
        });
    </script>
@endsection
