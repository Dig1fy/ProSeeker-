function RenderTheImage() {
    let inputImg = document.querySelector('#upload');
    let imageRender = document.querySelector('#imageResult')

    if (inputImg.files.length > 0) {
        let newPictureSrc = URL.createObjectURL(inputImg.files[0]);
        imageRender.src = newPictureSrc;
    }
}