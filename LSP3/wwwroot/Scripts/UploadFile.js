const uploadButton = document.getElementById("uploadButton");

uploadButton.addEventListener("click", () => {
    const fileInputs = [
        document.getElementById("fileInput1"),
        document.getElementById("fileInput2"),
        document.getElementById("fileInput3")
    ];

    fileInputs.forEach(input => {
        const files = input.files;

        if (files.length > 0) {
            const formData = new FormData();
            for (let i = 0; i < files.length; i++) {
                formData.append("files[]", files[i]);
            }

            // Replace with your actual server-side upload endpoint
            const uploadUrl = "/your-upload-endpoint";

            fetch(uploadUrl, {
                method: "POST",
                body: formData
            })
                .then(response => {
                    // Handle the server response (e.g., show success/error messages)
                })
                .catch(error => {
                    console.error("Upload error:", error);
                });
        }
    });
});
