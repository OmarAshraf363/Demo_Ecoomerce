// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function allConfirm(id) {
    let form = document.getElementById(id);

    fetch(form.action, {
        method: form.method,
        body: new FormData(form),
        headers: { 'X-Requested-With': 'XMLHttpRequest' }
    }).then(response => response.json())
        .then(data => {
            if (data.isvalid) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Your work has been Done",
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    form.submit();
                });
            } else {

                form.querySelectorAll("span.text-danger").forEach(span => {
                    span.innerHTML = "";
                });


                for (let fieldName in data.nameErrors) {
                    let field = document.querySelector(`#${id} #${fieldName}`);
                    if (field) {
                        let span = field.nextElementSibling;
                        if (span && span.classList.contains('text-danger')) {
                            span.innerHTML = data.nameErrors[fieldName].join("<br>");
                        }
                    }
                }
            }
        });
}






//function addToCart(productId) {
   
//    fetch(`/Admin/Order/AddToCart/${productId}`, {
//        method: 'GET'
//    })
//        .then(response => response.json())
//        .then(data => {
//            if (data.isvalid) {
//                alert('Product added to cart successfully!');
//            } else {
//                alert('Failed to add product to cart.');
//            }
//        })
//        .catch(error => {
//            console.error('Error:', error);
//        });
//}



//Start Section Of Delet Item And Shoe Alert
function showAlert(categoryId, controler,area) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Your work has been saved",
                showConfirmButton: false,
                timer: 1500
            }).then(() => {

                window.location.href = `/${area}/${controler}/Delete/${categoryId}`;

            });
        }
    });
}
//End Section Of Delet Item And Shoe Alert


//Start Edit And Alerts
function Edit(name, id, modalName) {
    document.getElementById('editCategoryName').value = name;
    document.getElementById('editCategoryId').value = id;
    var editModal = new bootstrap.Modal(document.getElementById(modalName));
    editModal.show();
}
//End Edit And Alerts
function EditBrand(brand) {
    document.getElementById('Name').value = brand.name;
    document.getElementById('brandid').value = brand.id;
    var editModal = new bootstrap.Modal(document.getElementById("editModal"));
    editModal.show();
}



function AddToCart(productId, check) {
 

    if (!check) {
        //var modalLogin = new bootstrap.Modal(document.getElementById("Login"));
        //modalLogin.show();
        window.location.href = `identity/account/login`;
    } else {

    var modal = new bootstrap.Modal(document.getElementById("addtocart"));
    modal.show();
    document.getElementById("pId").value = productId;
    }
    


    
}

$(document).ready(function () {

    setTimeout(function () {
        $("#successmsg").fadeOut("slow");
    }, 4000);
});



function openModal(id, controllerName, modalname, actionName) {
    let url;
    if (id === null || id === undefined) {
        url = `/Admin/${controllerName}/${actionName}`;
    } else {
        url = `/Admin/${controllerName}/${actionName}/${id}`;
    }
    $.ajax({
        url: url,
        type: 'GET',
        success: function (data) {
            $(`#${modalname} .modal-content`).html(data);
            $(`#${modalname}`).modal('show');
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}

function openModalStock(id1, id2, controllerName, modalname, actionName) {
    let url;

    // Check if storeId or productId are null or undefined
    if (id1 === null || id1 === undefined || id2 === null || id2 === undefined) {
        url = `/Admin/${controllerName}/${actionName}`;
    } else {
        // Include both storeId and productId in the URL for the edit action
        url = `/Admin/${controllerName}/${actionName}?storeId=${id1}&productId=${id2}`;
    }

    $.ajax({
        url: url,
        type: 'GET',
        success: function (data) {
            $(`#${modalname} .modal-content`).html(data);
            $(`#${modalname}`).modal('show');
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}


