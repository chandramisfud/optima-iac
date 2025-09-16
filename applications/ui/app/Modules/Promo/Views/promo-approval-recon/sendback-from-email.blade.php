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
</head>
<body>
<div class="card_form"></div>
</body>
<script src="{{ asset('assets/plugins/global/plugins.bundle.js') }}"></script>
<script src="{{ asset('assets/js/scripts.bundle.js') }}"></script>
<script src="{{ asset('assets/plugins/custom/jquery-ui/jquery-ui.js') }}"></script>
<script>
    var swalTitle = "Promo Send Back";
    var param;
    $(document).ready(async function () {
        let url_str = new URL(window.location.href);
        param = url_str.searchParams.get("p");

        let dataPromo = await getData(param);

        if (dataPromo.error) {
            Swal.fire({
                title: swalTitle,
                text: "Link is not valid",
                icon: "warning",
                buttonsStyling: !1,
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-optima"},
                allowOutsideClick: false,
                allowEscapeKey: false,
            });
        } else {
            await Swal.fire({
                title: "Are you sure to send back this Promo " + dataPromo.refId + "?",
                text: "Write down sendback reason in the following box",
                input: 'text',
                type: 'warning',
                inputAttributes: {
                    autocapitalize: 'off'
                },
                showCancelButton: true,
                confirmButtonText: 'Sendback',
                showLoaderOnConfirm: true,
                allowOutsideClick: false,
                allowEscapeKey: false,
                preConfirm: (value) =>{
                    if(value){
                        return value
                    }else{
                        Swal.showValidationMessage(
                            `Request failed: enter notes`
                        )
                    }
                }
            }).then(async function (result) {
                if (result.isConfirmed) {
                    let notes = result.value
                    let url = '/promo/approval-recon/email/send-back/submit'
                    let formData = new FormData();
                    formData.append('promoId', dataPromo.promoId);
                    formData.append('profileId', dataPromo.profileId);
                    formData.append('nameApprover', dataPromo.nameApprover);
                    formData.append('notes', notes);
                    $.get('/refresh-csrf').done(function(data) {
                        $('meta[name="csrf-token"]').attr('content', data)
                        $.ajaxSetup({
                            headers: {
                                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
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
                            beforeSend: function() {

                            },
                            success: function(result, status, xhr, $form) {
                                if (!result.error) {
                                    Swal.fire({
                                        title: swalTitle,
                                        text: result.message,
                                        icon: "success",
                                        confirmButtonText: "OK",
                                        allowOutsideClick: false,
                                        allowEscapeKey: false,
                                        customClass: {confirmButton: "btn btn-optima"}
                                    });
                                } else {
                                    Swal.fire({
                                        title: swalTitle,
                                        text: result.message,
                                        icon: "error",
                                        confirmButtonText: "OK",
                                        allowOutsideClick: false,
                                        allowEscapeKey: false,
                                        customClass: {confirmButton: "btn btn-optima"}
                                    });
                                }
                            },
                            complete: function() {

                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                console.log(jqXHR.message)
                                Swal.fire({
                                    title: swalTitle,
                                    text: "Failed to sendback promo",
                                    icon: "error",
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: {confirmButton: "btn btn-optima"}
                                });
                            }
                        });
                    });
                }
            });
        }

    });

    const getData = (param) => {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: "/promo/approval-recon/email/data/id",
                type: "GET",
                dataType: 'json',
                data: {param:param},
                async: true,
                success: function (result) {
                    return resolve(result);
                },
                complete: function () {
                    return resolve();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                    return reject(errorThrown);
                }
            });
        }).catch((e) => {
            console.log(e);
        });
    }
</script>
</html>
