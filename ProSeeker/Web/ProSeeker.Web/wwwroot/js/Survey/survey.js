(function () {
    // Make first quesiton visible
    document.querySelector('.question-1').style.display = "block";
    document.querySelector(`#currentQ-1`).style.backgroundColor = "#2f8328";
})()

function GetPreviousQuestion(currentQuestionNum) {
    // Check if there are any previous questions. If any, hide current one and show the previous
    if (currentQuestionNum > 1) {
        emptyProgressCircle(currentQuestionNum);
        document.querySelector(`.question-${currentQuestionNum}`).style.display = "none";

        let previousQuestionNumber = Number(currentQuestionNum) - 1;
        document.querySelector(`.question-${previousQuestionNumber}`).style.display = "block";
    }
}

function LoadNextQuestion(currentQuestionNum, numberOfQuestion, event) {
    let currentAnswer = event.target;
    if (currentAnswer.nodeName !== "LABEL") {
        return;
    }

    // Hide current question
    let currentQuestion = document.querySelector(`.question-${currentQuestionNum}`);
    currentQuestion.style.display = "none";

    // Unhide next question if there is any. Otherwise, show the end of the survey and hide the progress circles
    if (currentQuestionNum >= numberOfQuestion) {
        document.querySelector('#survey-progress').style.display = "none";
        document.querySelector('.endOfQuizAppearance').style.display = "block";
        return;

    } else {
        let nextQuestionNumber = Number(currentQuestionNum) + 1;
        fillProgressCircle(nextQuestionNumber)

        let nextQuestion = document.querySelector(`.question-${nextQuestionNumber}`);
        nextQuestion.style.display = "block";
    }
}

function fillProgressCircle(index) {
    document.querySelector(`#currentQ-${index}`).style.backgroundColor = "#2f8328"
}

function emptyProgressCircle(index) {
    document.querySelector(`#currentQ-${index}`).style.backgroundColor = "white"
}

function exitSurvey() {
    window.location.replace("/");
}