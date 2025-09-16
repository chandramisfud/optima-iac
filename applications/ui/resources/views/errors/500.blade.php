<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <meta name="csrf-token" content="{{ csrf_token() }}">
    <title>Coming Soon - Optima SN</title>

    <link rel="stylesheet" type="text/css" href="{{ asset('assets/plugins/global/plugins.bundle.css') }}" />
    <link rel="stylesheet" type="text/css" href="{{ asset('assets/css/style.bundle.css') }}" />
    

    <!-- Custom fonts for this template -->
    <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:200,200i,300,300i,400,400i,600,600i,700,700i,900,900i" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css?family=Merriweather:300,300i,400,400i,700,700i,900,900i" rel="stylesheet">
    {{-- <link href="{{ asset('/vendor/fontawesome-free/css/all.min.css') }}" rel="stylesheet" type="text/css" /> --}}

    <!-- Custom styles for this template -->
    <link href="{{ asset('/assets/css/error/coming-soon.min.css') }}" rel="stylesheet" type="text/css" />

</head>
<script>
  $.ajaxSetup({
      headers: {
          'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
      }
  });
</script>

<body>

    <div class="overlay"></div>
    <video playsinline="playsinline" autoplay="autoplay" muted="muted" loop="loop">
        <source src="{{ asset('/assets/media/mp4/bg.mp4') }}" type="video/mp4">
    </video>

    <div class="masthead">
        <div class="masthead-bg">
            <div class="container h-100">
                <div class="row h-100">
                    <div class="col-12 my-auto">
                        <div class="masthead-content text-white py-5 py-md-0">
                            <h1 class="mb-3 text-white">Server Error | 500</h1>
                            <p class="mb-5 text-white">Please Contact </br>
                            <strong><a href="http://xvautomation.com" target="_blank" class="kt-link">PT. XVAutomation Indonesia</a></strong></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

  <!-- Bootstrap core JavaScript -->
  {{-- <script src="{{ asset('/vendor/jquery/jquery.min.js') }}" type="text/javascript"></script> --}}
  {{-- <script src="{{ asset('/vendor/bootstrap/js/bootstrap.bundle.min.js') }}" type="text/javascript"></script> --}}

  <!-- Custom scripts for this template -->
  {{-- <script src="{{ asset('/js/coming-soon.min.js') }}" type="text/javascript"></script> --}}
</body>

</html>
