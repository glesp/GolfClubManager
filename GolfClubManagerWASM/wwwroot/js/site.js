window.showToast = (toastId) => {
    let toastElement = document.getElementById(toastId);
    if (toastElement) {
        let toast = new bootstrap.Toast(toastElement);
        toast.show();
    }
};
