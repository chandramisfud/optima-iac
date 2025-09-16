@section('title', @$title)
    <!DOCTYPE html>
<html lang="en">
<head><base href="">
    <title>Optima SN - @yield('title')</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Content-Security-Policy" content="default-src *; style-src * 'unsafe-inline';
        file: blob: *;
        img-src 'self' data: file: blob: *;
        style-src 'self' 'unsafe-inline' file: blob: *;
        worker-src 'self' 'unsafe-inline' file: blob: *;
        script-src 'self' 'unsafe-inline' 'unsafe-eval' *;">
    <meta charset="utf-8" />
    <meta property="og:locale" content="en_US" />
    <meta property="og:type" content="article" />
    <meta property="og:title" content="Optima SN - PT. Danone" />
    <meta property="og:url" content="https://xvautomation.com" />
    <meta property="og:site_name" content="Optima SN" />
    <meta name="csrf-token" content="{{ csrf_token() }}">
    <link rel="shortcut icon" href="{{ asset('assets/media/logos/logo.ico') }}" />
    <!--begin::Fonts-->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700" />
    <!--end::Fonts-->

    @include('panels.style')

    <style>
        .swal2-popup {
            width: 50%;
        }

        .swal2-html-container {
            text-align: justify;
        }

        .swal2-title {
            font-size: 14pt;
        }

        .swal2-icon.swal2-question {
            border-color: #5867dd;
            color: #5867dd;
        }

        .swal2-icon {
            margin: .5em auto .5em;
        }

        .swal2-popup .swal2-title {
            font-size: 18pt;
            color: #181c32;
            font-weight: bolder;
        }

        .swal2-styled.swal2-confirm {
            font-size: 12pt;
        }

        .swal2-styled.swal2-cancel {
            font-size: 12pt;
        }
    </style>
</head>
<body>
</body>
<script src="{{ asset('assets/plugins/global/plugins.bundle.js') }}"></script>
<script src="{{ asset('assets/js/scripts.bundle.js') }}"></script>
<script src="{{ asset('assets/plugins/custom/jquery-ui/jquery-ui.js') }}"></script>
<script>
    let swalTitle = "Budget Mass Approval";
    let param, a, c;

    $(document).ready(async function () {
        let url_str = new URL(window.location.href);
        param = url_str.searchParams.get("i");
        a =  url_str.searchParams.get("a");
        c = url_str.searchParams.get("c");

        let text = `<span style="font-size: 12pt;">I hereby acknowledged that by approving this, I have thoroughly checked the request and I’m authorizing the Promo ID initiators to sign contract with the relevant external party in accordance with the details outlined in the Promo ID. I understand all the consequences that may arise as a result of giving approval without careful checking and I am willing to accept the consequences that arise as a result of approving without careful checking.</span>`;
        let textConfirm = "Yes, approve";
        let url = '/budget/approval-request/approve';
        if (a === "reject") {
            text = `<div style="text-align: center"><span style="font-size: 12pt; text-align: center">Are you sure to reject?</span></div>`;
            url = '/budget/approval-request/reject';
            textConfirm = "Yes, reject";
        }

        Swal.fire({
            title: swalTitle,
            html: text,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#5867dd',
            cancelButtonColor: '#AAAAAA',
            allowOutsideClick: false,
            allowEscapeKey: false,
            showLoaderOnConfirm: true,
            cancelButtonText: 'No, cancel',
            confirmButtonText: textConfirm,
            reverseButtons: true,
            preConfirm: async () => {
                return new Promise((resolve) => {
                    let formData = new FormData();
                    formData.append('i', param);
                    formData.append('c', c);
                    $.get('/refresh-csrf').done(function(data) {
                        let elMeta = $('meta[name="csrf-token"]');
                        elMeta.attr('content', data)
                        $.ajaxSetup({
                            headers: {
                                'X-CSRF-TOKEN': elMeta.attr('content')
                            }
                        });
                        $.ajax({
                            url         : url,
                            data        : formData,
                            type        : 'POST',
                            async       : true,
                            dataType    : 'JSON',
                            cache       : false,
                            contentType : false,
                            processData : false,
                            success: function(result) {
                                return resolve(result);
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                console.log(errorThrown);
                                return resolve({error: true, message: errorThrown});
                            }
                        });
                    });
                });
            },
        }).then(function (result) {
            if (result.isConfirmed) {
                let value = result.value;
                if (!value.error) {
                    Swal.fire({
                        title: value.message,
                        icon: "success",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        close();
                    });
                } else {
                    Swal.fire({
                        title: value.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        close();
                    });
                }
            } else {
                close();
            }
        });
    });
</script>
</html>
