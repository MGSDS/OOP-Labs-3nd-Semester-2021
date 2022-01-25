Лабораторная 0. Isu

Цель: ознакомиться с языком C#, базовыми механизмами ООП. В шаблонном репозитории описаны базовые сущности, требуется реализовать недостающие методы и написать тесты, которые бы проверили корректность работы.

Предметная область. Студенты, группы, переводы (хоть где-то), поиск. Группа имеет название (соответсвует шаблону M3XYY, где X - номер курса, а YY - номер группы). Студент может находиться только в одной группе. Система должна поддерживать механизм перевода между группами, добавления в группу и удаление из группы.

Требуется реализовать предоставленный в шаблоне интерфейс:

public interface IIsuService
{
    Group AddGroup(GroupName name);
    Student AddStudent(Group group, string name);

    Student GetStudent(int id);
    Student FindStudent(string name);
    List<Student> FindStudents(GroupName groupName);
    List<Student> FindStudents(CourseNumber courseNumber);

    Group FindGroup(GroupName groupName);
    List<Group> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(Student student, Group newGroup);
}
И протестировать написанный код:

[Test]
public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
{
}

[Test]
public void ReachMaxStudentPerGroup_ThrowException()
{
}

[Test]
public void CreateGroupWithInvalidName_ThrowException()
{
}

[Test]
public void TransferStudentToAnotherGroup_GroupChanged()
{
}
FAQ

Нужно использовать GroupName groupName или string groupName ? Можно использовать любой вариант. Первый предпочтительней.
Лабораторная 1. Shops

Цель: продемонстрировать умение выделять сущности и проектировать по ним классы.

Прикладная область: магазин, покупатель, доставка, пополнение и покупка товаров. Магазин имеет уникальный идентификатор, название (не обязательно уникальное) и адрес. В каждом магазине установлена своя цена на товар и есть в наличии некоторое количество единиц товара (какого-то товара может и не быть вовсе). Покупатель может производить покупку. Во время покупки - он передает нужную сумму денег магазину. Поставка товаров представляет собой набор товаров, их цен и количества, которые должны быть добавлены в магазин.

Тест кейсы:

Поставка товаров в магазин. Создаётся магазин, добавляются в систему товары, происходит поставка товаров в магазин. После добавления товары можно купить.
Установка и изменение цен на какой-то товар в магазине.
Поиск магазина, в котором партию товаров можно купить максимально дешево. Обработать ситуации, когда товара может быть недостаточно или товаров может небыть нигде.
Покупка партии товаров в магазине (набор пар товар + количество). Нужно убедиться, что товаров хватает, что у пользователя достаточно денег. После покупки должны передаваться деньги, а количество товаров измениться.
NB:

Можно не поддерживать разные цены для одного магазина. Как вариант, можно брать старую цену, если магазин уже содержит этот товар. Иначе брать цену указанную в поставке.
Пример ожидаемого формата тестов представлен ниже. Используемые в тестах API магазина/менеджера/etc не являются интерфейсом для реализации в данной лабораторной. Не нужно ему следовать 1 в 1, это просто пример.
public void SomeTest(moneyBefore, productPrice, productCount, productToBuyCount)
{
	var person = new Person("name", moneyBefore);
	var shopManager = new ShopManager();
	var shop = shopManager.Create("shop name", ...);
	var product = shopManager.RegisterProduct("product name");
	
	shop.AddProducts( ... );
	shop.Buy(person, ...);
	
	Assert.AreEquals(moneyBefore - productPrice  * productToBuyCount, person.Money);
	Assert.AreEquals(productCount - productToBuyCount , shop.GetProductInfo(product).Count);
}
Лабораторная 2. ISUExtra (<3 ОГНП)

Цель: научиться выделять зоны ответственности разных сущностей и проектировать связи между ними.

Предметнвая область: Реализация системы записи студентов на ОГНП.

Untitled

Курс ОГНП - дополнительные занятия, которые могут изучать студенты. Курс реализует определенный мегафакультет. Курс изучается в несколько потоков с ограниченным количеством мест. У каждого потока есть свое расписание - список пар, которые проводятся в течение недели. Пара - описание временного интервала в который группа занимается. Пара должна быть ассоциирована с группой, временем, преподавателем и аудиторией.

Студенты могут записываться на два разных курса ОГНП. Студент не может записаться на ОГНП, которое представляет мегафакультет его учебной группы. Учебная группы принадлежат определенному мегафакультету, который определятся из названия группы. Каждый учебная группа имеет список пар. При записи студента должна быть проверка на то, что пары его учебной группы не пересекаются с парами потока ОГНП.

Требуется реализовать функционал:

Добавление нового ОГНПBackupJob
Запись студента на опредленный ОГНП
Возможность снять запись
Получение потоков по курсу
Получение списка студентов в определенной группе ОГНП
Получение списка не записавшихся на курсы студентов по группе
Лабораторная 3. Backups

Цель: применить на практике принципы из SOLID, GRASP.

Предметная область

Бекап (Backup) — в общем случае, это резервная копия каких-то данных, которая делается для того, чтобы в дальнейшем можно было восстановить эти данные, то есть откатиться до того момента, когда она была создана. В контексте данной системы, бекапом обозначим связанную цепочку созданных точек.

Точка восстановления (Restore point) — резервная копия объектов, созданная в определенный момент. Описать можно датой создания и список резервных копий объектов, которые бекапились в момент создания точки.

Бекапная джоба (Backup job) - сущность, которая содержит информацию о конфигурации создаваемых бекапов (список файлов, которые нужно бекапить, способ хранения и прочее) и о уже созданных точках данного бекапа. Также отвечает за создание новых точек восстановления.

Объект джобы (Job object) - объекты, которые добавлены с бекапную джобу, для которых нужно создавать копии при процессинге джобы.

Сторадж (Storage) - файл, в котором хранится резервная копия объекта джобы, который был создан в конкретной точке.

Репозиторий (Repository) - абстракция над способом хранения бекапов. В рамках самого простого кейса, репозиторием будет некоторая директория на локальной файловой системе, где будут лежать стораджи.

Пример логики работы

Выполняем такие действия:

Создаём джобу, добавляем три объекта FileA FileB FileC
Запускаем джобу, получаем рестор поинт в котором есть стораджи FileA_1 FileB_1 FileC_1
Повторяем, получаем стораджи *_2
Убираем из бекапной джобы FileC, запускаем джобу, получаем третий рестор поинт у которого есть два стораджа - FileA_3 FileB_3
Untitled

Создание резервных копий

Под созданием резервной копии файла подразумевается создание копии файла в другом месте. Система должна поддерживать расширяемость в алгоритмах создания резервных копий. Требуется реализовать два алгоритма:

Алгоритм раздельного хранения (Split storages) — для каждого объекта, который добавлен в джобу, создается копия - zip файл, в котором лежит объект.
Алгоритм общего хранения (Single storage) — все указанные в бекапе объекты сохраняются в один архив.
Хранение копий

В лабораторной работе подразуемвается, что резервные копии будут создаваться локально на файловой системе. Но логика выполнения должна абстрагироваться от этого, должна быть введена абстракция - репозиторий (см. принцип DIP из SOLID). И, например, в тестах стоит реализовать хранение в памяти, иначе тесты будут создавать много мусора, будут требовать дополнительной конфигурации, а также могут начать внезапно падать. Ожидаемая структура:

Корневая директория
Директории джоб, которые лежат в корневой директории
Файлы резервных копий, которые лежат в директории джобы
Создание рестор поинтов

Backup job отвечает за создание новых точек восстановления (т.е. выступает неким фасадом инкапсулируя в себе логику). При создании backup job должна быть возможность задачать её название, способ или место хранения и алгоритм создания резервных копий файлов. Должна поддерживаться возможность добавлять или убирать Job objects из Backup Job. Результатом работы алгоритма является создание новой точки восстановления. Точка восстановления должна содержать как минимум информацию о том, какие объекты были в ней забекаплены.

Тест кейсы

Тест-1
Cоздаю бекапную джобу
Указываю Split storages
Добавляю в джобу два файла
Запускаю создание точки
Удаляю один из файлов
Запускаю создание
Проверяю, что создано две точки и три стораджа
Тест-2, который лучше оформлять не тестом т.к. посмотреть нормально можно только на настоящей файловой системе
Cоздаю бекапную джобу, указываю путь директории для хранения бекапов
Указываю Single storage
Добавляю в джобу два файла
Запускаю создание точки
Проверяю, что созданы директории и файлы
Про тесты

Список тест-кейсов не является необходимым минимумом по покрытию тестов, это только некоторые из примеров. Стоит учитывать, что тест, которые напрямую работают с файловой системой, будут не работать на CI. Нужно делать такие реализации репозиториев, которые позволят тестам не падать.

QnA

Зачем нам вообще нужен сторадж и в чем коцептуальное различие между бекапом?

Бекапы - это абстрактное понятие, которое описывает концепцию реализации. Стораджи - это уже детали реализации процесса создания бекапов.

Что такое репозиторий и какие проблемы он решает?

В описании лабораторной, репозиторий - это абстракция над тем, куда и как будет записана копия файла. В самом простом случае, мы рассматривает файловую систему как репозиторий. Другие сущности (например, джоба) не должны напрямую работать с файловой системой, вызывать методы создания файла или директории. Вся эта логика выполняется за интерфейсом. Это позволит соответствовать DIP и OCP, легко добавить другие реализации, которые, например, будут сохранять копии сразу на гугл диск.

Лабораторная 4. Banks

Цель: применить на практике принципы из SOLID, GRASP, паттерны

Предметная область

Есть несколько Банков, которые предоставляют финансовые услуги по операциям с деньгами.

В банке есть Счета и Клиенты. У клиента есть имя, фамилия, адрес и номер паспорта (имя и фамилия обязательны, остальное – опционально).

Счета и проценты

Счета бывают трёх видов: Дебетовый счет, Депозит и Кредитный счет. Каждый счет принадлежит какому-то клиенту.

Дебетовый счет – обычный счет с фиксированным процентом на остаток. Деньги можно снимать в любой момент, в минус уходить нельзя. Комиссий нет.

Депозитный счет – счет, с которого нельзя снимать и переводить деньги до тех пор, пока не закончится его срок (пополнять можно). Процент на остаток зависит от изначальной суммы, например, если открываем депозит до 50 000 р. - 3%, если от 50 000 р. до 100 000 р. - 3.5%, больше 100 000 р. - 4%. Комиссий нет. Проценты должны задаваться для каждого банка свои.

Кредитный счет – имеет кредитный лимит, в рамках которого можно уходить в минус (в плюс тоже можно). Процента на остаток нет. Есть фиксированная комиссия за использование, если клиент в минусе.

Комиссии

Периодически банки проводят операции по выплате процентов и вычету комиссии. Это значит, что нужен механизм проматывания времени, чтобы посмотреть, что будет через день/месяц/год и т.п.

Процент на остаток начисляется ежедневно от текущей суммы в этот день, но выплачивается раз в месяц (и для дебетовой карты и для депозита). Например, 3.65% годовых. Значит в день: 3.65% / 365 дней = 0.01%. У клиента сегодня 100 000 р. на счету - запомнили, что у него уже 10 р. Завтра ему пришла ЗП и стало 200 000 р. За этот день ему добавили ещё 20 р. На следующий день он купил себе новый ПК и у него осталось 50 000 р. - добавили 5 р. Таким образом, к концу месяца складываем все, что запоминали. Допустим, вышло 300 р. - эта сумма добаляется к счету или депозиту в текущем месяце.

Разные банки предлагают разные условия. В каждом банке известны величины процентов и комиссий.

Центральный банк

Регистрацией всех банков, а также взаимодействием между банками занимается центральный банк. Он должен управлять банками (предоставлять возможность создать банк) и предоставлять необходимый функционал, чтобы банки могли взаимодействовать с другими банками (например, можно реализовать переводы между банками через него). Он также занимается уведомлением других банков о том, что нужно начислять остаток или комиссию - для этого механизма не требуется создавать таймеры и завязываться на реальное время.

Операции и транзакции

Каждый счет должен предоставлять механизм снятия, пополнения и перевода денег (то есть счетам нужны некоторые идентификаторы).

Еще обязательный механизм, который должны иметь банки - отмена транзакций. Если вдруг выяснится, что транзакция была совершена злоумышленником, то такая транзакция должна быть отменена. Отмена транзакции подразумевает возвращение банком суммы обратно. Транзакция не может быть повторно отменена.

Создание клиента и счета

Клиент должен создаваться по шагам. Сначала он указывает имя и фамилию (обязательно), затем адрес (можно пропустить и не указывать), затем паспортные данные (можно пропустить и не указывать).

Если при создании счета у клиента не указаны адрес или номер паспорта, мы объявляем такой счет (любого типа) сомнительным, и запрещаем операции снятия и перевода выше определенной суммы (у каждого банка своё значение). Если в дальнейшем клиент указывает всю необходимую информацию о себе - счет перестает быть сомнительным и может использоваться без ограничений.

Обновление условий счетов

Для банков требуется реализовать методы изменений процентов и лимитов не перевод. Также требуется реализовать возможность пользователям подписываться на информацию о таких изменениях - банк должен предоставлять возможность клиенту подписаться на уведомления. Стоит продумать расширяемую систему, в которой могут появится разные способы получения нотификаций клиентом (да, да, это референс на тот самый сайт). Например, когда происходит изменение лимита для кредитных карт - все пользователи, которые подписались и имеют кредитные карты, должны получить уведомление.

Консольный интерфейс работы

Для взаимодействия с банком требуется реализовать консольный интерфейс, который будет взаимодействовать с логикой приложения, отправлять и получать данные, отображать нужную информацию и предоставлять интерфейс для ввода информации пользователем.

Дополнения

На усмотрение студента можно ввести свои дополнительные идентификаторы для пользователей, банков etc.
На усмотрение студента можно пользователю добавить номер телефона или другие характеристики, если есть понимание зачем это нужно.
QnA

Q: Нужно ли предоставлять механизм отписки от информации об изменениях в условии счетов A: Не обговорено, значит на ваше усмотрение (это вообще не критичный момент судя по условию лабы)

Q: Транзакциями считаются все действия со счётом, или только переводы между счетами. Если 1, то как-то странно поддерживать отмену операции снятия, а то после отмены деньги удвоятся: они будут и у злоумышлениика на руках и на счету. Или просто на это забить A: Все операции со счетами - транзакции.

Q: Фиксированная комиссия за использование кредитного счёта, когда тот в минусе измеряется в % или рублях, и когда её начислять: после выполнения транзакции, или до. И нужно ли при отмене транзакции убирать и начисленную за неё комиссию. A: Фиксированная комиссия означает, что это фиксированная сумма, а не процент. Да, при отмене транзакции стоит учитывать то, что могла быть также комиссия.

Q: Если транзакция подразумевает возвращение суммы обратно - но при этом эта же сумма была переведена на несколько счетов (пример перевод денег со счета 1 на счёт 2, со счёта 2 на счёт 3) Что происходит если клиент 1 отменяет транзакцию? Подразумевается ли что деньги по цепочке снимаются со счёта 3? (на счету 2 их уже физически нет) Либо у нас банк мошеннический и деньги "отмываются" и возмещаются клиенту 1 с уводом счёта 2 в минус A: Банк не мошеннический, просто упрощённая система. Транзакции не связываются между собой. Так что да, можно считать, что может уйти в минус.

Лабораторная 5. BackupsExtra

Теормин

Слиянияе точек восстановления (мердж) - процесс слияния двух точек в результате которого получается одна точка

Сохранение и загрузка данных

Система должна уметь загружать свое состояние после перезапуска программы. Это может быть реализовано за счет сохранения данных о настройках джоб в конфигурационный файл, который будет лежать в корневой директории. После загрузки ожидается, что в приложение загрузится информация о существующих джобах, добавленных в них объектах, информация о созданных точках восстановления.

Алгоритмы очистки точек

Помимо создания, нужно контролировать количество хранимых точек восстановления. Чтобы не допускать накопления большого количества старых и неактуальных точек, требуется реализовать механизмы их очистки — они должны контролировать, чтобы цепочка точек восстановления не выходила за допустимый лимит. В рамках лабораторной ожидается реализация таких типов лимитов:

По количеству рестор поинтов - ограничивает длину цепочки из рестор поинтов (храним последние N рестор поинтов и очищаем остальные)
По дате - ограничивает насколько старые точки будут хранится (очищаем все точки, которые были сделаны до указанной даты)
Гибрид - возможность комбинировать лимиты. Пользователь может указывать, как комбинировать:
нужно удалить точку, если она не подходит хотя бы под один установленный лимит
нужно удалить точку, если она не подходит за все установленные лимиты
Например, пользователь выбирает гибрид алгоритмов "по количеству" и "по дате". Если по одному из алгоритмов необходимо оставить точки P1 P2 P3, а по другому — P1 P2 P3 P4 P5, то в первом варианте останутся точки P1-P3, а во втором - P1-P5.

Если для соответствия лимита требуется удалить все точки - должна бросаться ошибка.

Мердж точек

Стоит разделять алгоритм выбора точек для удаления и процесс удаления. Требуется поддержать альтернативное поведение при выходе за лимиты - мердж точек. Мердж работает по правилам:

Если в старой точке есть объект и в новой точке есть объект - нужно оставить новый, а старый можно удалять
Если в старой точке есть объект, а в новоей его нет - нужно перенести его в новую точку
Если в точке объекты храняться по правилу Single storage, то старая точка просто удаляется
Логирование

Логика работы бекпов не должна напрямую завязываться на консоль или другие внешние компоненты. Чтобы поддержать возможность уведомлять пользователя о событиях внутри алгоритма, требуется реализовать интерфейс для логирования и вызывать его в нужных моментах. Например, писать что создается сторадж или рестор поинт. Задаваться способ логирования должен из-вне. Например, при создании джобы. В рамках системы ожидаются такие реализации логера:

Консольный, который логирует информацию в консоль
Файловый, который логирует в указанный файл
Для логирования сущностей стоит реализовать в самих сущностях методы, которые генерируют информативную строку, которая описывает сущность.

Для логера стоит поддержать возможность конфигурирации - указать нужно ли делать префикс с таймкодом в начале строки.

Восстановление

Целью создания бекапов является предоставление возможности восстановиться из резервной копии. Требуется реализовать функционал, который бы позволял указать Restore point и восстановить данные из него. Нужно поддержать два режима восстановления:

to original location - восстановить файл в то место, из которого они бекапились (и заменить, если они ещё существуют)
to different location - восстановить файл в указанную папку
Notes

Для проверки работоспособности алгоритма работы со временем нужно спроектировать систему так, чтобы в тестах была возможность созданным объектам задавать время создания.
Дополнения к лабораторным работам

Дополнительные задания выполняются и сдаются вместе с основной частью. Они должны быть сделаны до начала пары, когда стоит дедлайн.

Лабораторная 1. Shops

Консольный UI (можно упростить себе жизнь и взять https://github.com/spectreconsole/spectre.console)
Лабораторная 2. ISUExtra

Лабораторная 3. Backups

Проектирование представления кода - диаграм.
Реализация бекапов не только на локальный диск, но и на TCP-сервер.
Лабораторная 4. Banks

ORM
