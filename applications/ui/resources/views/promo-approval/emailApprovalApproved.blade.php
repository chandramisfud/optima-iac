@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/css/promo-approval.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')

@endsection

@section('content')

@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script>
        $(document).ready(function () {

            $.ajaxSetup({
                headers: {
                    'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                }
            });

            let refId = "{{ @$refId}}";
            let promoId = "{{ @$promoId}}";
            let userApprover = "{{ @$userApprover}}";
            let recon = "{{ @$recon}}";

            if(recon==1){
                var url = '/promoapproval/emailApprovalRecon/submit'
            }else{
                var url = '/promoapproval/emailApproval/submit'
            }

            let data = {notes: '', approvalStatusCode: 'TP2', promoId: promoId, userApprover: userApprover};
            $.ajax({
                url         : url,
                data        : data,
                type        : 'POST',
                async       : true,
                dataType    : 'JSON',
                cache       : false,
                contentType : false,
                processData : false,
                success: function(result, status, xhr, $form) {
                    if (!result.error) {
                        Swal.fire({
                            title: 'Promo Approved',
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            if (!result.error) {
                                window.close();
                            }
                        });
                    } else {
                        Swal.fire({
                            title: 'Warning',
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "Confirm",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            window.close();
                        });
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.message)
                    Swal.fire({
                        title: swalTitle,
                        text: "Failed to save data, an error occurred in the process",
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            });
        });
    </script>
@endsection
