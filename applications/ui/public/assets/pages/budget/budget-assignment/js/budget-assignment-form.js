'use strict';

var validator, validator_dtl, method, method_dtl, trIndexEditDtl, dt_budget_assignment_dtl;
var id, budgetId, allocationId, frownId;
var swalTitle = "Budget Assignment";

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    let dialerObject = new KTDialer(dialerElement, {
        min: 1900,
        step: 1,
    });
    dialerObject.setValue(new Date().getFullYear());

    Inputmask({
        alias: "numeric",
        allowMinus: false,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#budgetAmountDtl, #budgetAssigned");


    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#budgetAmount");

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    id = url_str.searchParams.get("id");

    if (method === 'update') {
        $('#btn_search_budget_source').addClass('d-none');
        $('#dialer_period').html('<input type="text" class="form-control form-control-sm form-control-solid-bg" name="period" id="period" autocomplete="off" readonly/>');
        blockUI.block();
        disableButtonSave();
        Promise.all([ getData(id) ]).then(async () => {
            $('#txt_info_method').text('Edit');
            enableButtonSave();
            blockUI.release();
        });
    }

    validator = FormValidation.formValidation(document.getElementById('form_budget_assignment'), {
        fields: {
            period: {
                validators: {
                    notEmpty: {
                        message: "Budget year must be enter"
                    }
                }
            },
            longDesc: {
                validators: {
                    notEmpty: {
                        message: "Select a budget source"
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });

    validator_dtl = FormValidation.formValidation(document.getElementById('form_budget_assignment_dtl'), {
        fields: {
            profileId: {
                validators: {
                    notEmpty: {
                        message: "Select an Assign Name"
                    }
                }
            },
            description: {
                validators: {
                    notEmpty: {
                        message: "Description must be enter"
                    },
                }
            },
            budgetAmount: {
                validators: {
                    notEmpty: {
                        message: "Budget Amount must be enter"
                    },
                    greaterThan: {
                        min: 1,
                        message: "Budget Amount must greater than 0"
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });

    dt_budget_assignment_dtl = $('#dt_budget_assignment_dtl').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-5'i><'col-sm-7'p>>",
        processing: false,
        serverSide: false,
        paging: false,
        ordering: false,
        searching: false,
        scrollX: true,
        scrollY: "40vh",
        autoWidth: false,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Action',
                width: 20,
                orderable: false,
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return '<button class="btn btn-sm btn-icon btn-clean btn-optima btn_edit_dtl"><span class="fa fa-edit"></span></button>' +
                        '<button class="btn btn-sm btn-icon btn-clean btn-optima btn_delete_dtl"><span class="fa fa-trash"></span></button>';
                }
            },
            {
                targets: 1,
                title: 'Assign Name',
                data: 'ownId',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 2,
                title: 'Description',
                data: 'desc',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Amount',
                data: 'budgetAmount',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (typeof data === "string") {
                        return data;
                    } else {
                        return formatMoney(data, 0);
                    }
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {
            const intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };
            let api = this.api();
            let total = api.column(3).data().reduce(function (a, b) {
                return intVal(a) + intVal(b);
            }, 0);

            $('#budgetAmount').val(total);
        },
    });

    $("#dt_budget_assignment_dtl").on('click', '.btn_edit_dtl', function () {
        method_dtl = "edit";
        let tr = this.closest("tr");
        let trdata = dt_budget_assignment_dtl.row(tr).data();
        trIndexEditDtl = dt_budget_assignment_dtl.row(tr).index();
        $('#profileId').val(trdata.ownId).trigger('change');
        $('#description').val(trdata.desc);
        $('#budgetAmountDtl').val(trdata.budgetAmount);
        $('.modal-footer').css('display', 'flex');
        $('#btn_add_dtl').html('<span class="la la-check"></span> Update').after('<button class="btn btn-sm btn-secondary ms-2" id="btn_cancel_dtl">Cancel</button>');


        $('#btn_cancel_dtl').on('click', function () {
            formDetailReset();
            $(this).remove();
        });
    });

    $("#dt_budget_assignment_dtl").on('click', '.btn_delete_dtl', function () {
        let tr = this.closest("tr");
        let trIndex = dt_budget_assignment_dtl.row(tr).index();
        dt_budget_assignment_dtl.row(trIndex).remove().draw();
        formDetailReset();
        $('#btn_cancel_dtl').remove();
    });

});

$('#btn_search_budget_source').on('click', function () {
    $('#dt_list_budget_allocation').val('');
    let period = $('#period').val();
    dt_budget_source_list.clear().draw();
    dt_budget_source_list.ajax.url("/budget/assignment/list/source?period=" + period).load();
    $('#modal_list_budget_source').modal('show');
});


$('#btn_add_dtl').on('click', function () {
    if (method_dtl === "edit") {
        update_dtl();
    } else {
        add_dtl();
    }
});

$('#btn_save').on('click', function () {
    validator.validate().then(function (status) {
        let trdata = dt_budget_assignment_dtl.rows().data();
        let elBudgetAssigned = $('#budgetAssigned');
        let elBudgetAmount = $('#budgetAmount');
        if (status === "Valid") {
            if (trdata.length > 0) {
                let budgetAssigned = parseFloat(elBudgetAssigned.val().toString().replace(/,/g, ''));
                let budgetAmount = parseFloat(elBudgetAmount.val().toString().replace(/,/g, ''));
                if (budgetAmount !== budgetAssigned) {
                    Swal.fire({
                        title: swalTitle,
                        text: "The value of budget assigned and budget amount must be the same",
                        icon: "warning",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-primary"}
                    });
                } else {
                    let e = document.querySelector("#btn_save");
                    let data_detail = [];
                    for (let i = 0; i < trdata.length; i++) {
                        data_detail.push({
                            refId: "",
                            assignmentId: 0,
                            ownId: trdata[i]['ownId'],
                            desc: trdata[i]['desc'],
                            budgetAmount: trdata[i]['budgetAmount'],
                            budgetSourceId: budgetId,
                            periode: "",
                        });
                    }
                    let formData = new FormData($('#form_budget_assignment')[0]);
                    formData.append('budgetId', budgetId);
                    formData.append('frownId', frownId);
                    formData.append('allocationId', allocationId);
                    formData.append('assignmentDetail', JSON.stringify(data_detail));
                    let url = '/budget/assignment/save';
                    if (method === "update") {
                        formData.append('id', id);
                        url = '/budget/assignment/update';
                    }

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
                                e.setAttribute("data-kt-indicator", "on");
                                e.disabled = !0;
                            },
                            success: function(result, status, xhr, $form) {
                                if (!result.error) {
                                    Swal.fire({
                                        title: swalTitle,
                                        text: result.message,
                                        icon: "success",
                                        confirmButtonText: "OK",
                                        customClass: {confirmButton: "btn btn-optima"}
                                    }).then(function () {
                                        window.location.href = '/budget/assignment';
                                    });
                                } else {
                                    Swal.fire({
                                        title: swalTitle,
                                        text: result.message,
                                        icon: "error",
                                        confirmButtonText: "OK",
                                        customClass: {confirmButton: "btn btn-optima"}
                                    });
                                }
                            },
                            complete: function() {
                                e.setAttribute("data-kt-indicator", "off");
                                e.disabled = !1;
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                console.log(jqXHR.message)
                                Swal.fire({
                                    title: swalTitle,
                                    text: "Failed to save data, an error occurred in the process",
                                    icon: "error",
                                    confirmButtonText: "OK",
                                    customClass: {confirmButton: "btn btn-optima"}
                                });
                            }
                        });
                    });
                }
            } else {
                Swal.fire({
                    title: swalTitle,
                    text: "Please add detail budget assign",
                    icon: "warning",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-primary"}
                });
            }
        }
    });
});

const add_dtl = () => {
    validator_dtl.validate().then(function (status) {
        if (status === "Valid") {
            let ownId = $('#profileId').val();
            let description = $('#description').val();
            let budgetAmount = $('#budgetAmountDtl').val();

            let detail = [];
            detail['ownId'] = ownId;
            detail['desc'] = description;
            detail['budgetAmount'] = parseFloat(budgetAmount.toString().replace(/,/g, ''));
            formDetailReset();
            dt_budget_assignment_dtl.row.add(detail).draw();
        }
    });
}

const update_dtl = () => {
    validator_dtl.validate().then(function (status) {
        if (status === "Valid") {
            dt_budget_assignment_dtl.row(trIndexEditDtl).every(function () {
                let ownId = $('#profileId').val();
                let description = $('#description').val();
                let budgetAmount = $('#budgetAmountDtl').val();
                let detail = this.data();
                detail['ownId'] = ownId;
                detail['desc'] = description;
                detail['budgetAmount'] = parseFloat(budgetAmount.toString().replace(/,/g, ''));
                formDetailReset();

                this.invalidate();
            });
            $('#btn_cancel_dtl').remove();
            dt_budget_assignment_dtl.draw();
        }
    });
}

const formDetailReset = () => {
    $('#profileId').val('').trigger('change');
    $('#description').val('');
    $('#budgetAmountDtl').val('');
    method_dtl = "add";
    $('#btn_add_dtl').html('<span class="la la-plus"></span> Add');
}

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/budget/assignment/get-data/id",
            type: "GET",
            data: {id:id},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    let values = result.data;
                    allocationId = values.allocationId;
                    frownId = values.frownId;
                    $('#period').val(values.periode ?? "");
                    $('#longDesc').val(values.budgetSource);
                    $('#fromOwnerName').val(values.frownName);
                    $('#entityLongDesc').val(values.entity);
                    $('#budgetAssigned').val(values.budgetAmount);
                    if (values.assignmentId) {
                        dt_budget_assignment_dtl.rows.add(values.assignmentId).draw();
                    }
                    if (values.ownerId) {
                        let ownerId = values.ownerId;
                        let data = [];
                        for (let j = 0, len = ownerId.length; j < len; ++j){
                            data.push({
                                id: ownerId[j],
                                text: ownerId[j]
                            });
                        }
                        $('#profileId').select2({
                            placeholder: "Select a User",
                            width: '100%',
                            data: data
                        });
                    }
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete:function(){
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}
