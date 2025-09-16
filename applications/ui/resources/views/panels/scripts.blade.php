<!--begin::Global Javascript Bundle(used by all pages)-->
<script src="{{ asset('assets/plugins/global/plugins.bundle.js') }}"></script>
<script src="{{ asset('assets/js/scripts.bundle.js') }}"></script>
<script src="{{ asset('assets/plugins/custom/jquery-ui/jquery-ui.js') }}"></script>
<script src="{{ asset('assets/plugins/custom/flatpickr/flatpickr-id.js') }}"></script>
<script src="{{ asset('assets/js/get-menu-id.js') }}"></script>
<script src="{{ asset('assets/js/switch-profile.js') }}"></script>
<script src="{{ asset('assets/js/custom.js?v=' . microtime()) }}"></script>
<script src="{{ asset('assets/js/search.js?v=' . microtime()) }}"></script>
<!--end::Global Javascript Bundle-->

<!--begin::Vendor Javascript -->
@yield('vendor-script')
<!--end::Vendor Javascript-->

<!--begin::Page Javascript -->
@yield('page-script')
<!--end::Page Javascript-->
