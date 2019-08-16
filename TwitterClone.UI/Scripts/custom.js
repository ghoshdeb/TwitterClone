
function countChar(val) {
    var max = 140;
    var len = val.value.length;
    if (len >= max) {
        $('#charNum').text(' you have reached the limit');
    } else {
        var char = max - len;
        $('#charNum').text(char + ' characters left');
    }
};
function GetAll() {
    var activeUser = $("#currentUser").data('value');
    $.ajax({
        url: "GetAllTweet",
        type: "GET",
        datatype: "json",
        success: function (data) {
            $.each(data, function (i, v) {
                var editClass = '';
                var deleteClass = '';
                if (activeUser != v.user_id) {
                    editClass = 'editHideClass';
                    deleteClass = 'deleteHideClass';
                }
                var date = v.created;
                var tweetdate = new Date(parseInt(date.substr(6)));
                var row = '';
                row = '<tr row_id="' + v.tweet_id + '">';
                row += '<td >' + v.user_id + '</td>';
                row += '<td ><div class="row_data" edit_type="click" col_name="message">' + v.message + '</div></td>';
                row += '<td >' + tweetdate.toDateString() + '</td>';

                //--->edit options > start
                row += '<td>';
                
                row += '<span class="btn_edit ' + editClass + '"> <a href="#" class="btn btn-link " row_id="' + v.tweet_id + '" > Edit</a> </span > ';
                row += '<span class="btn_delete ' + deleteClass + '"> <a href="#" class="btn btn-link"  row_id="' + v.tweet_id + '"> Delete</a></span>';
                row += '<span class="btn_save"> <a href="#" class="btn btn-link"  row_id="' + v.tweet_id + '"> Save</a> | </span>';
                row += '<span class="btn_cancel"> <a href="#" class="btn btn-link" row_id="' + v.tweet_id + '"> Cancel</a> | </span>';
                row += '</td>';
                //--->edit options > end
                row += '</tr>';
                $('#t1').append(row);
                //$('#hideClass').hide()
                if (activeUser != v.user_id) {
                    $('.editHideClass').hide();
                    $('.deleteHideClass').hide();
                }
                $('.btn_save').hide();
                $('.btn_cancel').hide();

            });
        },
        error: function (xhr) {
            alert("error occurs")
        }
    });
}

$(document).ready(function () {
    GetAll();
    $("#update").click(function () {
        let msg = $("#message").val();

        $.ajax({
            url: "AddTweet",
            type: "POST",
            data: { message: msg },
            dataType: "json",
            success: function (data) {
                $("#message").val("");
                $("#t1").html('')
                GetAll();
            },
            error: function (XRH) {
                alert("error occurs")
            }
        });
    });
    $(function () {
        $("#searchFollowing").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: 'SearchFollowing',
                    data: "{ 'name': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return item;
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
        });
    });
    $(function () {
        $("#searchUnFollowing").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: 'SearchUnFollowing',
                    data: "{ 'name': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return item;
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
        });
    });
})

//--->button > edit > start	
$(document).on('click', '.btn_edit', function (event) {
    event.preventDefault();
    var tbl_row = $(this).closest('tr');

    var row_id = tbl_row.attr('row_id');

    tbl_row.find('.btn_save').show();
    tbl_row.find('.btn_cancel').show();
    //hide edit button
    tbl_row.find('.btn_edit').hide();
    tbl_row.find('.btn_delete').hide();

    //make the whole row editable
    tbl_row.find('.row_data')
        .attr('contenteditable', 'true')
        .attr('edit_type', 'button')
        .addClass('bg-warning')
        .css('padding', '3px')
    //--->add the original entry > start
    tbl_row.find('.row_data').each(function (index, val) {
        //this will help in case user decided to click on cancel button
        $(this).attr('original_entry', $(this).html());
    });
    //--->add the original entry > end

});
//--->button > edit > end


//--->button > cancel > start	
$(document).on('click', '.btn_cancel', function (event) {
    event.preventDefault();

    var tbl_row = $(this).closest('tr');

    var row_id = tbl_row.attr('row_id');

    //hide save and cacel buttons
    tbl_row.find('.btn_save').hide();
    tbl_row.find('.btn_cancel').hide();

    //show edit button
    tbl_row.find('.btn_edit').show();

    //make the whole row editable
    tbl_row.find('.row_data')
        .attr('edit_type', 'click')
        .removeClass('bg-warning')
        .css('padding', '')

    tbl_row.find('.row_data').each(function (index, val) {
        $(this).html($(this).attr('original_entry'));
    });
});
//--->button > cancel > end


//--->save whole row entery > start	
$(document).on('click', '.btn_save', function (event) {
    event.preventDefault();
    var tbl_row = $(this).closest('tr');
    var row_id = tbl_row.attr('row_id');
    //hide save and cacel buttons
    tbl_row.find('.btn_save').hide();
    tbl_row.find('.btn_cancel').hide();
    //show edit button
    tbl_row.find('.btn_edit').show();
    //make the whole row editable
    tbl_row.find('.row_data')
        .attr('edit_type', 'click')
        .removeClass('bg-warning')
        .css('padding', '')

    //--->get row data > start
    var arr = {};
    tbl_row.find('.row_data').each(function (index, val) {
        var col_name = $(this).attr('col_name');
        var col_val = $(this).html();
        arr[col_name] = col_val;
    });

    //ajax call
    $.extend(arr, { tweet_id: row_id });
    $.ajax({
        url: "UpdateTweet",
        type: "POST",
        data: arr,
        dataType: "json",
        success: function (data) {
            $("#message").val("");
            $("#t1").html('')
            GetAll();
        },
        error: function (XRH) {
            alert("error occurs")
        }
    });

});
$(document).on('click', '.btn_delete', function (event) {
    event.preventDefault();
    var tbl_row = $(this).closest('tr');
    var row_id = tbl_row.attr('row_id');
    //hide save and cacel buttons
    tbl_row.find('.btn_save').hide();
    tbl_row.find('.btn_cancel').hide();
    //show edit button
    tbl_row.find('.btn_edit').show();
    //make the whole row editable
    tbl_row.find('.row_data')
        .attr('edit_type', 'click')
        .removeClass('bg-warning')
        .css('padding', '')

    //--->get row data > start
    var arr = {};
    tbl_row.find('.row_data').each(function (index, val) {
        var col_name = $(this).attr('col_name');
        var col_val = $(this).html();
        arr[col_name] = col_val;
    });

    //ajax call
    $.extend(arr, { tweet_id: row_id });
    var Con = confirm("are you sure ?");
    if (Con == true) {
        $.ajax({
            url: "DeleteTweet",
            type: "POST",
            data: arr,
            dataType: "json",
            success: function (data) {
                $("#t1").html('')
                GetAll();
            },
            error: function (XRH) {
                alert("error occurs")
            }
        });
    }

});
$(document).on('click', '.btn_delete_profile', function (event) {
    var activeUser = $("#currentUser").data('value');
    var con = confirm("are you sure to delete the profile ?");
    if (con == true) {
        $.ajax({
            url: "deleteprofile",
            type: "post",
            data: { userid: activeUser },
            datatype: "json",
            success: function (data) {
                window.location.href = "/User/Login";
            },
            error: function (xrh) {
                alert("error occurs")
            }
        });
    }

});
$(document).on('click', '.btn-following', function (event) {
    $.ajax({
        url: "Unfollowing",
        type: "GET",
        datatype: "json",
        success: function (data) {
            $.each(data, function (i, v) {
                var row = '';
                row = '<div row_id="' + v.user_id + '">';
                row += '<div >' + v.user_id + '</div>';

                //--->edit options > start
                row += '<span class="btn_unfollow" > <a href="#" class="btn btn-link " row_id="' + v.user_id + '" > Unfollow</a> </span>';


                //--->edit options > end
                $('#collapse1').append(row);

            });
        },
        error: function (xhr) {
            alert("error occurs")
        }
    });

});
