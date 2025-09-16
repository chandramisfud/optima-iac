'use strict';

var swalTitle = "Upload Debit Note";
var dt_dn_upload_attachment;
var heightContainer = 630;
$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    new Dropzone("#dropzone_dn", {
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        },
        type: 'POST',
        url: "/dn/upload-attachment/temp", // Set the url for your upload script location
        paramName: "file",
        maxFilesize: 10, // MB
        addRemoveLinks: true,
        init: function() {
            this.on("error", function(file, message) {
                this.removeFile(file);
            });
            this.on("addedfile", function(file) {
                setTimeout(function() {
                    if (file.size === 0 || file.size > 10000000) {
                        swal.fire({
                            title: 'File Size must not be 0 bytes and  should not exceed 10MB',
                            type: 'error',
                            // showCancelButton: true,
                            confirmButtonText: 'OK'
                        }).then(function (result) {
                            if (result.value) {
                                var fileName = file.name;
                                $.ajax({
                                    headers: {
                                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                                    },
                                    type: 'POST',
                                    url: '/dn/upload-attachment/temp-delete',
                                    data: {name: fileName, request: 'delete'},
                                    // data: {filename: name},
                                    success: function (data) {
                                        //console.log("File has been successfully removed!!");
                                    },
                                    error: function (e) {
                                        //console.log(e);
                                    }
                                });
                                let fileRef;
                                return (fileRef = file.previewElement) != null ? fileRef.parentNode.removeChild(file.previewElement) : void 0;
                            }
                        });
                    }
                }, 1000);

                // Get error
                this.on("uploadprogress", function(file, progress) {
                });

                $('.dz-remove').on('click', function() {
                    console.log('test');
                    var fileName = file.name;
                    $.ajax({
                        headers: {
                            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                        },
                        type: 'POST',
                        url: '/dn/upload-attachment/temp-delete',
                        data: {name: fileName,request: 'delete'},
                        success: function (data){
                            console.log("File has been successfully removed!!");
                        },
                        error: function(e) {
                            console.log(e);
                        }});
                    var fileRef;
                    return (fileRef = file.previewElement) != null ?
                        fileRef.parentNode.removeChild(file.previewElement) : void 0;
                });
            })
        },
        success: function(file, response)
        {
            console.log(response);
        },
        error: function(file, response)
        {
            return false;
        }
    });
    $('.dropzone').css('background-color', '#fff !important');

    dt_dn_upload_attachment = $('#dt_dn_upload_attachment').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        // order: [[1, 'asc']],
        // ajax: {
        //     url: url_datatable,
        //     type: 'get',
        // },
        ordering: false,
        processing: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                width: 250,
                title: 'DN Number',
                data: 'dnNumber',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.resultDN === 'OK') {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-check" style="color:green"></i>';
                        } else {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-times" style="color:red"></i>';
                        }

                    }
                }
            },
            {
                targets: 1,
                title: 'Attachment 1',
                width: 170,
                data: 'attach1',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result1 === 'OK') {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-check" style="color:green"></i>';
                        } else {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-times" style="color:red"></i>';
                        }
                    }
                }
            },
            {
                targets: 2,
                title: 'Attachment 2',
                width: 170,
                data: 'attach2',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result2 === 'OK') {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-check" style="color:green"></i>';
                        } else {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-times" style="color:red"></i>';
                        }
                    }
                }
            },
            {
                targets: 3,
                title: 'Attachment 3',
                width: 170,
                data: 'attach3',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result3 === 'OK') {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-check" style="color:green"></i>';
                        } else {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-times" style="color:red"></i>';
                        }
                    }
                }
            },
            {
                targets: 4,
                title: 'Attachment 4',
                width: 170,
                data: 'attach4',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result4 === 'OK') {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-check" style="color:green"></i>';
                        } else {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-times" style="color:red"></i>';
                        }
                    }
                }
            },
            {
                targets: 5,
                title: 'Attachment 5',
                width: 170,
                data: 'attach5',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result5 === 'OK') {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-check" style="color:green"></i>';
                        } else {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-times" style="color:red"></i>';
                        }
                    }
                }
            },
            {
                targets: 6,
                title: 'Attachment 6',
                width: 170,
                data: 'attach6',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result6 === 'OK') {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-check" style="color:green"></i>';
                        } else {
                            return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-times" style="color:red"></i>';
                        }
                    }
                }
            },
            {
                targets: 7,
                title: 'Result',
                width: 170,
                data: 'resultProgress',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data === 'success') {
                        return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-check" style="color:green"></i>';
                    } else {
                        return '<span class="fs-12px me-1">' + data + '</span><i class="fa fa-times" style="color:red"></i>';
                    }
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        Swal.fire({
            text: strmessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"},
            closeOnConfirm: false,
            showLoaderOnConfirm: false,
            closeOnClickOutside: false,
            closeOnEsc: false,
            allowOutsideClick: false,

        });
    };
});

$('#btn_download').on('click', function() {
   let url = "/dn/upload-attachment/download-template";

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_upload').on('click', function() {
    let e = document.querySelector("#btn_upload");
    let url = '/dn/upload-attachment/importData';
    $.ajax({
        url: url,
        type: 'POST',
        async: true,
        dataType: 'JSON',
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function () {
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
        },
        success: function (result, status, xhr, $form) {
            if (!result.error) {
                Swal.fire({
                    title: 'Upload Result',
                    html: "<div style='font-size:18;'>Success  : <b>" + result.resSuccesSum + "</b><br>Failed  : <b>" + result.resFailedSum + "</b></div><br><p>Short by column result to see failed upload</p><p>Check filename if there are failed result and refer to Special Character Rules</p>",
                    icon: "info",
                    confirmButtonText: "Ok",
                    allowOutsideClick: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(async function () {
                    $("#detail_result").removeClass("d-none");
                    dt_dn_upload_attachment.clear().draw();
                    dt_dn_upload_attachment.rows.add(result.data).draw();
                    Dropzone.forElement("#dropzone_dn").removeAllFiles(true);
                });
            } else {
                Swal.fire({
                    title: swalTitle,
                    text: result.message,
                    icon: "warning",
                    confirmButtonText: "Ok",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        },
        complete: function () {
            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.message)
            Swal.fire({
                title: swalTitle,
                text: "Files Upload Failed",
                icon: "error",
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});
