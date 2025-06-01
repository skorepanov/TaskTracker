import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../stores/RootStore";
import TaskListItem from "./TaskListItem";
import NoTasks from "./NoTasks";
import TaskCreationModalButton from "./TaskCreationModalButton";

const TodayTaskList: React.FC = observer(() => {
    const { taskStore, folderStore } = useStore();

    const incompletedTaskComponents = taskStore
        .getTodayIncompletedTasks()
        .map(t => (
            <TaskListItem
                key={t.id}
                task={t}
                shouldShowFolder={true}
                folder={folderStore.folders.find(f => f.id === t.folderId)}
            />
        ));

    const completedTaskComponents = taskStore
        .getTodayCompletedTasks()
        .map(t => (
            <TaskListItem
                key={t.id}
                task={t}
                shouldShowFolder={true}
                folder={folderStore.folders.find(f => f.id === t.folderId)}
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
