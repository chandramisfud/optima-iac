<div class="menu-sub menu-sub-accordion menu-active-bg">
@if(isset($submenus))
    @foreach($submenus as $submenu)
        @if(isset($submenu->submenu))
            <div data-kt-menu-trigger="click" class="menu-item menu-accordion
                @if (substr(Route::currentRouteName(), 0, strlen($submenu->slug)) == $submenu->slug)
                        show
                @endif
                ">
                <span class="menu-link">
                    <span class="menu-bullet">
                        <span class="{{ $submenu->icon }}"></span>
                    </span>
                    <span class="menu-title fw-bolder">{{ $submenu->name }}</span>
                    <span class="menu-arrow"></span>
                </span>
                @include('panels.submenu', [ 'submenus' => $submenu->submenu ])
            </div>
        @else
            <div class="menu-item">
                <a class="menu-link {{ substr(Route::currentRouteName(), 0, strlen($submenu->slug)) == $submenu->slug ? 'active' : '' }}" href="{{ $submenu->url }}" data-menuid="{{ $submenu->id }}">
                    <span class="menu-bullet">
                        <span class="{{ $submenu->icon }}"></span>
                    </span>
                    <span class="menu-title fw-bolder">{{ $submenu->name }}</span>
                </a>
            </div>
        @endif
    @endforeach
@endif
</div>
