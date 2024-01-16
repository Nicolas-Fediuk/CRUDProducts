const btnDeletes = document.querySelectorAll(".btnDelete");

btnDeletes.forEach(btn => {

    btn.addEventListener("click", function () {

        const namePro = $(this).closest("tr").find(".ids").val();

        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-success",
                cancelButton: "btn btn-danger"
            },
            buttonsStyling: false
        });
        swalWithBootstrapButtons.fire({
            title: "Are you sure you want to delete the product "+namePro+"?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, delete it!",
            cancelButtonText: "No, cancel!",
            reverseButtons: true
        })
            .then((result) => {
                if (result.isConfirmed) {

                    fetch('/Products/DeleteProduct', {
                        method: 'POST',
                        body: JSON.stringify(namePro),
                        headers: {
                            'Content-type': 'application/json'
                        }
                    })
                        .then(response => {
                            if (response.ok) {
                                return response.json();
                            } else {
                                throw new Error('Network response was not ok.');
                            }
                        })
                        .then(data => {
                            if (data.success) {
                                swalWithBootstrapButtons.fire({
                                    title: "Deleted",
                                    icon: "success"
                                }).then(() => {
                                    window.location.href = '/Products/Index';
                                });
                            }
                        })
                }
                else if (result.dismiss === Swal.DismissReason.cancel) {
                    swalWithBootstrapButtons.fire({
                        title: "Cancelled",
                        icon: "error"
                    });
                }
            })
    });
});