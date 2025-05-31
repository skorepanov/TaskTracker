import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../stores/RootStore";
import TaskListItem from "./TaskListItem";
import NoTasks from "./NoTasks";
import TaskCreationModalButton from "./TaskCreationModalButton";

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
            <div style={{ float: "left" }}>
                <strong>Задачи на сегодня</strong>
            </div>
            <div style={{ float: "right" }}>
                <TaskCreationModalButton dueDateTime={new Date()} />
            </div>
            <Divider />
            {incompletedTaskComponents.length > 0 ? (
                incompletedTaskComponents
            ) : (
                <NoTasks />
            )}
            {completedTaskComponents.length > 0 ? <Divider /> : null}
            {completedTaskComponents}
        </>
    );
});

export default TodayTaskList;
