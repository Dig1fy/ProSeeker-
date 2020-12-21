(function run() {

    // select all elements
    const html = {
        startCountRef: document.querySelector('.start-quiz'),
        quiz: document.querySelector("#quiz-content"),
        question: document.querySelector("#quiz-question"),
        qImg: document.querySelector("#quiz-qImg"),
        choiceA: document.querySelector("#quiz-answer-A"),
        choiceB: document.querySelector("#quiz-answer-B"),
        choiceC: document.querySelector("#quiz-answer-C"),
        choiceD: document.querySelector("#quiz-answer-D"),
        choiceContainer: document.querySelector('#quiz-choice-container'),
        progress: document.querySelector("#quiz-progress"),
        exitBtn: document.querySelector('.exit-quiz-wrapper'),
        backBtn: document.querySelector("#quiz-container > div.backward-btn-wrapper"),
        progressLoaderRef: document.querySelector('.progress-load')
    }

    let lastQuestion = questions.length - 1;
    let runningQuestion = 0;

    //start counter
    (function () {
        let progressCount = 5;

        var timeleft = 5; //CHANGE THIS TO 5 WHEN FINISHED WITH TESTING!!!!!
        var downloadTimer = setInterval(function () {
            progressCount += 25;
            timeleft--;
            html.progressLoaderRef.style.width = progressCount + "%"
            document.getElementById("countdowntimer").textContent = timeleft;
            if (timeleft <= 0) {
                clearInterval(downloadTimer);
                html.startCountRef.style.display = "none";
                html.quiz.style.display = "block";
                html.backBtn.style.display = "block";
                document.querySelector('.quiz-intro').style.display = "none";
                document.querySelector('.progress-load-wrapper').style.display = "none";
            }

        }, 1000);
    })()

    //render a question
    function renderQuestion() {

        if (runningQuestion <= lastQuestion) {
            let currentQuestion = questions[runningQuestion]
            html.question.innerHTML = `<p class="text-center"> ${currentQuestion.question} </p>`;
            html.qImg.innerHTML = `<img src="${currentQuestion.imgSrc}">`;
            html.choiceA.textContent = currentQuestion.answerA;
            html.choiceB.textContent = currentQuestion.answerB;
            html.choiceC.textContent = currentQuestion.answerC;
            html.choiceD.textContent = currentQuestion.answerD;

        } else { //showing the backward btn and hides the quizz after the last question
            //html.backBtn.style.display = "none";
            html.quiz.innerHTML = `<h1 class="end-quiz"> Благодарим Ви за отделеното време! </h1>`;
            setTimeout(redirect, 2000)
        }
    }

    // render progress
    (function () {
        for (let qIndex = 0; qIndex <= lastQuestion; qIndex++) {
            html.progress.innerHTML += "<div class='prog' id=" + qIndex + "></div>";
        }
    })()

    //opening the next question (the listener is on the entire div container)
    html.choiceContainer.addEventListener('click', choiceHandler);
    function choiceHandler(e) {
        document.getElementById(runningQuestion).style.backgroundColor = "#0f0";
        runningQuestion++;
        renderQuestion();
    }

    function redirect() {
        window.location.replace("/");
    }

    html.exitBtn.addEventListener('click', redirect);
    renderQuestion()

    html.backBtn.addEventListener('click', openPreviousQuestion);

    //set the correct previous question with ternary operator
    function openPreviousQuestion() {
        runningQuestion = runningQuestion > 0 ? runningQuestion - 1 : 0
        // document.getElementById(runningQuestion).style.backgroundColor = "rgb(240, 237, 237)";
        document.getElementById(runningQuestion).style.backgroundColor = "white";
        renderQuestion();
    }


})()


 //let questions = [
    //    {
    //        question: "Как разбрахте са нашата платформа?",
    //        answerA: "От приятел",
    //        answerB: "Реклама в друг сайт",
    //        answerC: "Случайно, докато си сърфирах в мрежата",
    //        answerD: "Реклама по телевизията",
    //        imgSrc: "../images/quiz/1.png",
    //    },
    //    {
    //        question: "Какво в нашата платформа не Ви допада?",
    //        answerA: "Менюто е доста объркващо",
    //        answerB: "Цветовете не са взаимно съвместими",
    //        answerC: "Не разбирам, как точно работи",
    //        answerD: "Всичко е добре направено и ми допада",
    //        imgSrc: "../images/quiz/2.png"
    //    },
    //    {
    //        question: "Каква функционалност бихте искали да добавим?",
    //        answerA: "Възможност за качване на постове ",
    //        answerB: "По-детайлен профил на потребителите",
    //        answerC: "Повече категории / видове специалисти",
    //        answerD: "Не мога да преценя",
    //        imgSrc: "../images/quiz/3.png"
    //    },
    //    {
    //        question: "Как бихте оценили нашата платформа?",
    //        answerA: "Ужасна",
    //        answerB: "Нищо особено",
    //        answerC: "Добра",
    //        answerD: "Страхотна",
    //        imgSrc: "../images/quiz/4.png"
    //    }
    //]