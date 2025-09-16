@extends('layouts/layoutMaster')

@section('title', 'Form User Group Menu Configuration')

@section('vendor-style')
    <link href="{{ asset('/assets/plugins/custom/jstree/jstree.bundle.css') }}" rel="stylesheet" type="text/css" />
@endsection

@section('page-style')
    <link href="{{ asset('/assets/pages/useraccess/user-group/css/usergroupmenu-form.css') }}" rel="stylesheet" type="text/css" />
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method"></small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-save-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_groupmenu_config" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <div id="tree-container" class="tree-demo"></div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/jstree/jstree.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/pages/useraccess/user-group/js/usergroupmenu-form.js?v=2') }}"></script>
@endsection
