<html lang="en-US">
<title>Preview Attachment {{ @$title }}</title>
<div class="container-fluid d-flex">
    @if ($isExist)
        <iframe src="{{ asset($path) }}" width="100%" height="100%"></iframe>
    @else
        <iframe src="{{ asset('assets/media/logos/none.pdf') }}" width="100%" height="100%"></iframe>
    @endif
</div>
</html>
