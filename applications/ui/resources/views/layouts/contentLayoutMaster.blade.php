<body id="kt_body" class="page-loading-enabled page-loading header-fixed header-tablet-and-mobile-fixed toolbar-enabled toolbar-fixed aside-enabled aside-fixed footer-fixed"
      style="--kt-toolbar-height:45px;--kt-toolbar-height-tablet-and-mobile:45px">

@include('panels.loader')

<!--begin::Main-->
<!--begin::Root-->
<div class="d-flex flex-column flex-root">
    <!--begin::Page-->
    <div class="page d-flex flex-row flex-column-fluid">

        @include('panels.sidebar')

        <!--begin::Wrapper-->
        <div class="wrapper d-flex flex-column flex-row-fluid" id="kt_wrapper">

            @include('panels.header')

            <!--begin::Content-->
            <div class="content d-flex flex-column flex-column-fluid" id="kt_content">

                @yield('toolbar')

                <!--begin::Post-->
                <div class="post d-flex flex-column-fluid" id="kt_post">

                    <!--begin::Container-->
                    <div id="kt_content_container" class="container-fluid">
                    @yield('content')
                    </div>
                    <!--end::Container-->

                </div>
                <!--end::Post-->
            </div>
            <!--end::Content-->

            @include('panels.footer')

        </div>
        <!--end::Wrapper-->
    </div>
    <!--end::Page-->
</div>

{{--  @include('panels.popup-login')  --}}
@include('panels.popup-profile-change')

<!--end::Root-->
@include('panels.scroll-top')
<!--end::Main-->

</body>
@include('panels.scripts')
<!--end::Body-->
</html>
