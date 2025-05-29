import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../stores/RootStore";
import TaskListItem from "./TaskListItem";

const TodayTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTaskComponents = taskStore
        .getTodayIncompletedTasks()
        .map(t => (
            <TaskListItem
                key={`today-${t.id}`}
                task={t}
            />
        ));

    const completedTaskComponents = taskStore
        .getTodayCompletedTasks()
        .map(t => (
            <TaskListItem
                key={`today-${t.id}`}
                task={t}
            />
        ));

    return (
        <>
            <strong>Задачи на сегодня</strong>
            <Divider />
            {incompletedTaskComponents}
            <Divider />
            {completedTaskComponents}
        </>
    );
});

export default TodayTaskList;
