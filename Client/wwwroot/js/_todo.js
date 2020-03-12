$(document).ready(function () {
    var Status = $('#selectActivityStatus').val();
    $('#tblLists').on('keyup', function () {
        table
            .columns(3)
            .search(this.value)
            .draw();
    });
    var table = $("#tblLists").DataTable({
        "responsive": false,
        //"searching": false,
        "columnDefs": [{
            "targets": [0, 2, 3],
            "orderable": false,
            "searchable": false
        }],
        ajax: {
            url: "/ToDoLists/ListAll/" + Status,
            type: "GET"
        },
        "columns": [
            {
                "render": function (data, type, row) {
                    if (row.status == false) {
                        return '<button type="button" class="btn btn-secondary" id="Update" onclick="return UpdateStatus(' + row.id + ')"><i class="fa fa-square-o" title="Checklist"></i></button>';
                    } else {
                        return '<button type="button" class="btn btn-secondary" id="Uncheck" onclick="return UncheckStatus(' + row.id + ')"><i class="fa fa-check-square-o" title="Unchecklist"></i></button>';
                    }
                }
            },
            { "data": "name" },
            {
                "render": function (data, type, row) {
                    if (row.status == false) {
                        return "Active";
                    } else {
                        return "Completed";
                    }
                }
            },
            {
                "render": function (data, type, row) {
                    return '<a href="" id="Edit" class="edit" onclick="return GetbyID(' + row.id + ')" ><i class="material-icons" title="Edit Activity Name">&#xE254;</i></a>  <a href="" id="Delete" class="delete" onclick="return Delete(' + row.id + ')" ><i class="material-icons" title="Delete">&#xE872;</i></a>';
                }
            }
        ]
    });
});
$('#selectActivityStatus').change(function () {
    table.ajax.url('/ToDoLists/ListAll/' + $('#selectActivityStatus').val()).load();
});

//$(document).ready(function () {
//    var Status = $('#selectActivityStatus').val();
//    var no = 1;
//    debugger;
//    $('#tblLists').DataTable({
//        "aoColumns": [{ "bSortable": false }, null, { "bSortable": false }, { "bSortable": false }],
//        "responsive": true,
//        "ajax": {
//            url: "/ToDoLists/ListAll",
//            data: { status: Status },
//            type: "GET",
//        },
//        "columns": [
//            { "data": "Id"},
//            { "data": "Name" },
//            { "data": "Status" }
//        ]
//    });
//});
//$(document).ready(function () {
//    $('#tblLists').dataTable({
//        "aoColumns": [{ "bSortable": false }, null, { "bSortable": false }, { "bSortable": false }],
//        "ajax": loadDataAll(),
//        "responsive": true
//    });
//});
//Load Data function
//function loadDataAll() {
//    //var todolist = new Object();
//    //todolist.StatusActivity = $('#selectActivityStatus').val();
//    var Status = $('#selectActivityStatus').val();
//    $.ajax({
//        url: "/ToDoLists/ListAll",
//        data: { status: Status},
//        type: "GET",
//        async: false,
//        success: function (result) {
//            var html = '';
//            //var no = 1;
//            $.each(result, function (key, item) {
//            debugger;
//                html += '<tr>';
//                //html += '<td>'+ no++ +'</td>';
//                if (item.status == false) {
//                    html += '<td><button type="button" class="btn btn-secondary" id="Edit" onclick="return UpdateStatus(' + item.id + ')"><i class="fa fa-square-o" title="Checklist"></i></button></td > ';
//                } else {
//                    html += '<td><button type="button" class="btn btn-secondary" id="Edit" onclick="return UncheckStatus(' + item.id + ')"><i class="fa fa-check-square-o" title="Unchecklist"></i></button></td > ';
//                }
//                html += '<td>' + item.name + '</td>';
//                if (item.status == false) {
//                    html += '<td>Active</td>';
//                } else {
//                    html += '<td>Completed</td>';
//                }
//                html += '<td><button type = "button" class="btn btn-warning btn-sm" id="Edit" onclick="return GetbyID(' + item.id + ')" ><i class="fa fa-pencil" title="Edit Activity Name"></i></button> ' +
//                    '<button type = "button" class="btn btn-danger btn-sm" id="Delete" onclick="return Delete(' + item.id + ')" ><i class="fa fa-trash-o" title="Delete"></i></button></td > ';
//                html += '</tr>';
//            });
//            $('.tbody').html(html);
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
//    //$('div.dataTables_filter input', this.api().table().container());
//}
//function loadDataDone() {
//    $.ajax({
//        url: "/ToDoLists/ListDone",
//        type: "GET",
//        async: false,
//        success: function (result) {
//            var html = '';
//            $.each(result, function (key, item) {
//                html += '<tr>';
//                html += '<td><button type="button" class="btn btn-secondary" id="Edit" onclick="return UncheckStatus(' + item.id + ')"><i class="fa fa-check-square-o" title="Unchecklist"></i></button></td > ';
//                html += '<td>' + item.name + '</td>';
//                if (item.status == false) {
//                    html += '<td>Active</td>';
//                } else {
//                    html += '<td>Completed</td>';
//                }
//                html += '<td><button type = "button" class="btn btn-warning" id="Edit" onclick="return GetbyID(' + item.id + ')" ><i class="fa fa-pencil" title="Edit Activity Name"></i></button> ' +
//                    '<button type = "button" class="btn btn-danger" id="Delete" onclick="return Delete(' + item.id + ')" ><i class="fa fa-trash-o" title="Delete"></i></button></td > ';
//                html += '</tr>';
//            });
//            $('.tbody').html(html);
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
//}
//function loadDataUndone() {
//    $.ajax({
//        url: "/ToDoLists/ListUndone",
//        type: "GET",
//        async: false,
//        success: function (result) {
//            var html = '';
//            $.each(result, function (key, item) {
//                html += '<tr>';
//                html += '<td><button type="button" class="btn btn-secondary" id="Edit" onclick="return UpdateStatus(' + item.id + ')"><i class="fa fa-square-o" title="Unchecklist"></i></button></td > ';
//                html += '<td>' + item.name + '</td>';
//                if (item.status == false) {
//                    html += '<td>Active</td>';
//                } else {
//                    html += '<td>Completed</td>';
//                }
//                html += '<td><button type = "button" class="btn btn-warning" id="Edit" onclick="return GetbyID(' + item.id + ')" ><i class="fa fa-pencil" title="Edit Activity Name"></i></button> ' +
//                    '<button type = "button" class="btn btn-danger" id="Delete" onclick="return Delete(' + item.id + ')" ><i class="fa fa-trash-o" title="Delete"></i></button></td > ';
//                html += '</tr>';
//            });
//            $('.tbody').html(html);
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
//}

function GetbyID(id) {
    $('#Activity').css('border-color', 'lightgrey');
    //$('#Status').css('border-color', 'lightgrey');

    $.ajax({
        url: "/ToDoLists/GetbyID",
        data: { Id: id },
        type: "GET",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        success: function (result) {
            $('#Id').val(result[0]['id']);
            $('#Activity').val(result[0]['name']);
            //Status = result[0]['status'];
            //if (result[0]['status'] == false) {
            //    $("#Status option:contains(false)").attr('selected', 'selected');
            //}
            //else {
            //    $("#Status option:contains(true)").attr('selected', 'selected');
            //}

            $('#myModal').modal('show');
            $('#btnUpdate').show();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function Add() {
    if ($('#Name').val().trim() == "") {
        $('#Name').css('border-color', 'Red');
        $('#Name').focus();
        return false;
    }
    var todolist = new Object();
    todolist.Name = $('#Name').val();

    $.ajax({
        url: "/ToDoLists/Insert/",
        data: todolist,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            $('#Name').val("");
            $('#Name').css('border-color', 'lightgrey');
            //table.ajax.reload();
            //$('#tblLists').DataTable().ajax.reload();
            //$('#tblDoneLists').DataTable().ajax.reload();
            //$('#tblUndoneLists').DataTable().ajax.reload();
            //$('#tblLists').draw();
            //$('#tblDoneLists').draw();
            //$('#tblUndoneLists').draw();
            //loadDataAll();
            //loadDataDone();
            //loadDataUndone();
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Insert Successfully',
                showConfirmButton: false,
                timer: 1500
            });
            window.location.href = "/ToDoLists/Index";
        }
        else {
            Swal.fire('Error', 'Insert Failed', 'error');
            window.location.href = "/ToDoLists/Index";
        }
    });
}

//Function for getting the Data Based upon Employee ID
function UpdateStatus(Id) {
    //var todolist = new Object();
    //todolist.Id = Id;
    $.ajax({
        url: "/ToDoLists/UpdateStatus/",
        data: { id: Id },
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            //loadDataAll();
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 6000
            });
            window.location.href = "/ToDolists/Index";
        }
        else {
            Swal.fire('Error!', 'Update Failed.', 'error');
            window.location.href = "/ToDoLists/Index";
        }
    });
}

function UncheckStatus(Id) {
    //var todolist = new Object();
    //todolist.Id = Id;
    $.ajax({
        url: "/ToDoLists/UncheckStatus/",
        data: { id: Id },
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            ////loadDataAll();
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 6000
            });
            window.location.href = "/ToDolists/Index";
        }
        else {
            Swal.fire('Error!', 'Update Failed.', 'error');
            window.location.href = "/ToDoLists/Index";
        }
    });
}

//function for updating todolist's record
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var todolist = new Object();
    todolist.Id = $('#Id').val();
    todolist.Name = $('#Activity').val();
    //todolist.Status = $('#Status').val();

    $.ajax({
        url: "/ToDoLists/Update/",
        data: todolist,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            //loadDataAll();
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 6000
            });
            window.location.href = "/ToDolists/Index";
        }
        else {
            Swal.fire('Error!', 'Update Failed.', 'error');
            window.location.href = "/ToDoLists/Index";
        }
    })
}

//function for deleting employee's record
function Delete(ID) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/todolists/Delete/",
                data: { id: ID },
                type: "POST"
            }).then((result) => {
                if (result.statusCode == 200) {
                    //loadDataAll();
                    //loadDataDone();
                    //loadDataUndone();
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Delete Successfully',
                        showConfirmButton: false,
                        timer: 6000
                    });
                    window.location.href = "/ToDolists/Index";
                }
                else {
                    Swal.fire(
                        'Error!',
                        'Delete Failed.',
                        'error'
                    );
                    window.location.href = "/ToDolists/Index";
                }
            })
        }
    })
}
$(function () {
    $("[id*=btnSweetAlert]").on("click", function () {
        var id = $(this).closest('tr').find('[id*=id]').val();
        swal({
            title: 'Are you sure?' + ids,
            text: "You won't be able to revert this!" + id,
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
            confirmButtonClass: 'btn btn-success',
            cancelButtonClass: 'btn btn-danger',
            buttonsStyling: false
        }).then(function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/DeleteClick",
                    data: "{id:" + id + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        if (r.d == "Deleted") {
                            loadDataAll();
                        }
                        else {
                            swal("Data Not Deleted", r.d, "success");
                        }
                    }
                });
            }
        },
            function (dismiss) {
                if (dismiss == 'cancel') {
                    swal('Cancelled', 'No record Deleted', 'error');
                }
            });
        return false;
    });
});

function validate() {
    var isValid = true;
    if ($('#Activity').val().trim() == "") {
        $('#Activity').css('border-color', 'Red');
        $('#Activity').focus();
        isValid = false;
    }
    else {
        $('#Activity').css('border-color', 'lightgrey');
    }
    //if ($('#Status').val().trim() == "") {
    //    $('#Status').css('border-color', 'Red');
    //    $('#Status').focus();
    //    isValid = false;
    //}
    //else {
    //    $('#Status').css('border-color', 'lightgrey');
    //}
    return isValid;
}