import React from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../stores/RootStore";
import TaskListView from "./TaskListView";

const TodayTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTasks = taskStore.getTodayIncompletedTasks();
    const incompletedTaskComponents = incompletedTasks.map(t => (
        <TaskListView
            key={`today-${t.id}`}
            task={t}
        />
    ));

    const completedTaskComponents = taskStore
        .getTodayCompletedTasks()
        .map(t => (
            <TaskListView
                key={`today-${t.id}`}
                task={t}
            />
        ));

    return (
        <>
            <strong>
                Задачи на сегодня (не выполнено задач: {incompletedTasks.length}
                )
            </strong>
            {incompletedTaskComponents}
            {completedTaskComponents}
        </>
    );
});

export default TodayTaskList;
