<!--begin::Aside Menu-->
<div class="hover-scroll-overlay-y my-5 my-lg-5" id="kt_aside_menu_wrapper" data-kt-scroll="true" data-kt-scroll-activate="{default: false, lg: true}" data-kt-scroll-height="auto" data-kt-scroll-dependencies="#kt_aside_logo, #kt_aside_footer" data-kt-scroll-wrappers="#kt_aside_menu" data-kt-scroll-offset="0">
    <!--begin::Menu-->
    <div class="menu menu-column menu-title-white menu-state-title-primary menu-state-icon-primary menu-state-bullet-primary menu-arrow-white" id="#kt_aside_menu" data-kt-menu="true">
        <div class="menu-item">
            <div class="menu-content pb-2">
                <span class="menu-section text-white fw-bolder text-uppercase fs-8 ls-1">Menu</span>
            </div>
        </div>
        @if(isset($menuData[0]))
            @if(isset($menuData[0]->menu))
                @foreach($menuData[0]->menu as $menu)
                    @if(isset($menu->submenu))
                    <div data-kt-menu-trigger="click" class="menu-item menu-accordion
                        @foreach ($menu->submenu as $submenu)
                            @if (substr(Route::currentRouteName(), 0, strlen($menu->slug)) == $menu->slug)
                            show
                            @endif
                        @endforeach
                        ">
                        <span class="menu-link">
                            <span class="menu-icon">
                                <i class="{{ $menu->icon }}"></i>
                            </span>
                            <span class="menu-title fw-bolder">{{ $menu->name }}</span>
                            <span class="menu-arrow"></span>
                        </span>
                        @include('panels.submenu', [ 'submenus' => $menu->submenu ])
                    </div>
                    @else
                    <div class="menu-item">
                        <a class="menu-link {{ substr(Route::currentRouteName(), 0, strlen($menu->slug)) == $menu->slug ? 'active' : '' }}" href="{{ $menu->url }}" data-menuid="{{ $menu->id }}">
                        <span class="menu-icon">
                            <i class="{{ $menu->icon }}"></i>
                        </span>
                            <span class="menu-title fw-bolder">{{ $menu->name }}</span>
                        </a>
                    </div>
                    @endif
                @endforeach
            @endif
        @endif
    </div>
    <!--end::Menu-->
</div>
<!--end::Aside Menu-->
