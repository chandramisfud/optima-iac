<div class="toolbar" id="kt_toolbar">
    <div id="kt_toolbar_container" class="container-fluid d-flex">
        <div data-kt-swapper="true" data-kt-swapper-mode="prepend" data-kt-swapper-parent="{default: '#kt_content_container', 'lg': '#kt_toolbar_container'}" class="page-title d-flex align-items-center flex-wrap me-3 mb-5 mb-lg-0">
            @yield('breadcrumb')
        </div>
        <div class="d-flex py-1 flex-grow-1 justify-content-between">
            <div class="d-flex align-items-center py-1 ps-lg-5">
                <div class="d-none" id="btn_left_group">
                    @yield('button-toolbar-left')
                </div>
            </div>
            <div class="d-flex align-items-center py-1">
                @yield('button-toolbar-right')
            </div>
        </div>
    </div>
</div>
