var elements = [];  // Карточки теста
var answers = "";   // Ответы пользователя
var startTime = 40; // Время для ответа
var timer = startTime;  // Оставшееся время для ответа
var time;   // Ссылка на таймер
var secondPersent = 0; // Процент прошедшего времени

var DOMelement = document.getElementById("notAnswer")   // Получение отвтетов из бэка
var backAnswers = DOMelement.textContent;   // Получение отвтетов из бэка
DOMelement.remove();    // Удаление блока ответов

var timerVisual = document.getElementById("timerVisual");   // Визуал таймера

//------------Задаём стили-------------------
timerVisual.style.width=("100%");
timerVisual.style.background=("rgb(0,210,0");
timerVisual.style.textAlign="Center";
timerVisual.style.height=("8px");
//-------------------------------------------

var interval;   // Ссылка на таймер для визуала
var step = 100 / startTime / 60;    // Процент изменения шкалы за 1 кадр

getBlocksById();    // Получение блоков теста

// Получение блоков теста
function getBlocksById(){
    elements = document.getElementsByName('Question');
}

// Смена блоков теста с записью ответа
function changeQuestion(k){
    answers+=k; // Копим ответы
    var flag = false;    // Разрешение смены блока

    //Проходим по каждому блоку
    for (i = 0; i < elements.length; i++){
        // Если смена разрешена
        if (flag == true){
            elements[i].style.visibility='visible'; // Включаем блок
            
            // Двигаем bottom на высоту блока вопроса вниз
            document.getElementById("mainBlock").style.marginBottom=(elements[i].offsetHeight+"px");
            setTimer(); // Запускаем таймер
            secondPersent = 0;  // Обнуляем счетчик прошедшего времени в процентах
            break;  // выходим из цикла
        }
        // Если блок виден, то
        if ($(elements[i]).css("visibility") == "visible") {
            elements[i].style.visibility = "hidden";    // Прячем блок
            flag = true;   // Меняем флаг смены блока
            clearInterval(time);    // Отключаем таймеры
            clearInterval(interval);    // Отключаем таймеры
            timer = startTime;  // Обнуляем время для ответа
        }
    }
    // Если список вопросов закончился, то вызываем метод finish
    if (i==elements.length){
        finish();
    }
}

// Подготовка страницы для отправки Post запроса
function finish(){
    clearInterval(time);    // Отключаем таймер

    // Прячем все блоки
    elements.forEach(element => {
        element.style.visibility = "hidden";
    });

    // Показываем блок результата
    document.getElementById("Result").style.visibility="visible";
    result=0;   // счетчик правильных ответов

    // Сравниваем ответы
    for(j = 0; j < elements.length; j++){
        if (answers.charAt(j)==backAnswers.charAt(j)){
            result++;
        }
    }

    //---------------Записываем результаты---------------
    document.getElementById("countOfTrue").textContent = result + " правильных ответов из " + (elements.length);
    document.getElementById("testResult").value = "";
    result = 100 / j * result;
    document.getElementById("countOfTrueDB").value = parseInt(result, 10);
    //---------------Записываем результаты---------------
    let f = true;   // Флаг отсеивания
    switch(f){
        case (0<=result && result<20):
            document.getElementById("testResult").textContent = "Попробуйте еще раз немного позже. Мы уверены, Вы можете лучше";
            break;
        case (20<=result&& result<40):
            document.getElementById("testResult").textContent = "Нужно еще совсем немного. Мы уверены, Вы можете лучше";
            break;
        case (40<=result&& result<60):
            document.getElementById("testResult").textContent = "Неплохая попытка, попробуйте еще раз. Мы уверены, Вы можете лучше";
            break;
        case (60<=result&& result<80):
            document.getElementById("testResult").textContent = "Хороший результат. Можно попробовать его перебить";
            break;
        case (80<=result&& result<=100):
            document.getElementById("testResult").textContent = "Отлично!! Тест пройден. Попробуйте другие на главной странице";
            break;
    }
}

function setTimer(){
    time = setInterval(function(){
        if (timer <= 0){
            timerVisual.remove();
            clearInterval(interval);
            clearInterval(time);
            finish();
        }
        else{
            clearInterval(interval);
            interval = setInterval(frame, 1000/60);
        }
        timer--;
    }, 1000);

    function frame(){
        let width = $(timerVisual).css("width");
        width = parseInt(width, 10);
        if (width / 1 == 0){
            clearInterval(interval)
        }else{
            secondPersent += step;
            timerVisual.style.background='rgb('+(255-((100 - secondPersent)*2.55))+','+((100 - secondPersent)*2.10)+','+0+')';
            timerVisual.style.width = (100 - secondPersent)+"%";
        }
    }
}

function startTest(){
    elements[0].style.visibility="visible";
    document.getElementById("startCard").remove();
    setTimer();
}

