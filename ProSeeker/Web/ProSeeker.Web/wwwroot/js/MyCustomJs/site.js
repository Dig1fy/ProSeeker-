//function showOpinionsSection(parentId) {
//    // Show / hide opinions
//    let section = document.querySelector("#addOpinionsForm");
//    section.style.display = section.style.display === 'block' ? 'none' : 'block';

//    // Insert parentId value according to the opinion(comment) we want to reply
//    $("#addOpinionsForm input[name='ParentId']").val(parentId);

//    // Navigate to the text area
//    $([document.documentElement, document.body]).animate({
//        scrollTop: $("#addOpinionsForm").offset().top
//    }, 1000);
//}