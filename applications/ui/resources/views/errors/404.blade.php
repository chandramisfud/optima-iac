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
    <link href="{{ asset('/assets/css/coming-soon.css') }}" rel="stylesheet" type="text/css" />

</head>
<style>
    .mb-auto, .my-auto {
        margin-bottom: auto !important;
    }
    .mt-auto, .my-auto {
        margin-top: auto !important;
    }
    .col-12 {
        -ms-flex: 0 0 100%;
        flex: 0 0 100%;
        max-width: 100%;
    }
    .col, .col-1, .col-10, .col-11, .col-12, .col-2, .col-3, .col-4, .col-5, .col-6, .col-7, .col-8, .col-9, .col-auto, .col-lg, .col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-auto, .col-md, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-auto, .col-sm, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-auto, .col-xl, .col-xl-1, .col-xl-10, .col-xl-11, .col-xl-12, .col-xl-2, .col-xl-3, .col-xl-4, .col-xl-5, .col-xl-6, .col-xl-7, .col-xl-8, .col-xl-9, .col-xl-auto {
        position: relative;
        width: 100%;
        padding-right: 15px;
        padding-left: 15px;
    }

    .text-white {
        color: #fff !important;
    }

    .mt-auto, .my-auto {
        margin-top: auto !important;
    }
    .mb-auto, .my-auto {
        margin-bottom: auto !important;
    }
</style>
<script>
  $.ajaxSetup({
      headers: {
          'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
      }
  });
</script>

<body>

    <div class="overlay"></div>
    <video autoplay muted loop id="myVideo">
        <source src="{{ asset('/assets/media/mp4/bg.mp4') }}" type="video/mp4">
    </video>

    <div class="masthead">
        <div class="masthead-bg">
            <div class="container h-100">
                <div class="row h-100">
                    <div class="col-12 my-auto m-20">
                        <div class="masthead-content text-white py-0">
                            <h1 class="mb-3 text-white">Coming Soon!</h1>
                            <p class="mb-5 text-white">We're working hard to finish the development of this site. Please Contact </br>
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
