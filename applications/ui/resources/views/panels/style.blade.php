<!--begin::Global Stylesheets Bundle(used by all pages)-->
<link rel="stylesheet" type="text/css" href="{{ asset('assets/plugins/global/plugins.bundle.css') }}" />
<link rel="stylesheet" type="text/css" href="{{ asset('assets/css/style.bundle.css') }}" />
<!--end::Global Stylesheets Bundle-->

<!--begin::Vendor Stylesheets Bundle-->
@yield('vendor-style')
<!--end::Vendor Stylesheets Bundle-->

<!--begin::Override Stylesheets Bundle-->
<link rel="stylesheet" type="text/css" href="{{ asset('assets/css/custom.css?v=' . microtime()) }}" />
<!--end::Override Stylesheets Bundle-->

<!--begin::Page Stylesheets Bundle-->
@yield('page-style')
<!--end::Page Stylesheets Bundle-->
