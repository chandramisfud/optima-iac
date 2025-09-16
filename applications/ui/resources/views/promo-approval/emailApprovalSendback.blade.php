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

            let url;
            let refId = "{{ @$refId}}";
            let promoId = "{{ @$promoId}}";
            let userApprover = "{{ @$userApprover}}";
            let recon = "{{ @$recon}}";

            if(recon==1){
                url = '/promoapproval/emailApprovalRecon/submit'
            }else{
                url = '/promoapproval/emailApproval/submit'
            }

            let data = {notes: '', approvalStatusCode: 'TP2', promoId: promoId, userApprover: userApprover};

        });
    </script>
@endsection
