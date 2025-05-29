import React from "react";
import { useParams } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../stores/RootStore";
import TaskListView from "./TaskListView";

const FolderTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const params = useParams();
    const folderId = Number(params.id);

    const incompletedTasks = taskStore.incompletedTasks.filter(
        t => t.folderId === folderId
    );

    const incompletedTaskComponents = incompletedTasks.map(t => (
        <TaskListView
            key={`today-${t.id}`}
            task={t}
        />
    ));

    const completedTasks = taskStore.completedTasks.filter(
        t => t.folderId === folderId
    );

    const completedTaskComponents = completedTasks.map(t => (
        <TaskListView
            key={`today-${t.id}`}
            task={t}
        />
    ));

    return (
        <>
            <strong>
                Задачи из папки [{folderId}] (не выполнено задач:{" "}
                {incompletedTasks.length})
            </strong>
            {incompletedTaskComponents}
            <Divider />
            {completedTaskComponents}
        </>
    );
});

export default FolderTaskList;
