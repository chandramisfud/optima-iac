<div class="content d-flex flex-column flex-column-fluid" id="kt_content">
    @if (file_exists(public_path() . $pathPromo . str_replace('%20', ' ', $filename)))
        <iframe src="{{ @$pathPromo . $filename }}" width="100%" height="100%"></iframe>
    @else
        <iframe src="{{ asset('assets/media/logos/none.pdf') }}" width="100%" height="100%"></iframe>
    @endif
</div>
