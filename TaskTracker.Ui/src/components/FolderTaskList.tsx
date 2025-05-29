import React from "react";
import { useParams } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../stores/RootStore";
import TaskListItem from "./TaskListItem";

const FolderTaskList: React.FC = observer(() => {
    const { taskStore, folderStore } = useStore();

    const params = useParams();
    const folderId = Number(params.id);

    const folder = folderStore.folders.find(f => f.id === folderId)!;

    const incompletedTasks = taskStore.incompletedTasks.filter(
        t => t.folderId === folderId
    );

    const incompletedTaskComponents = incompletedTasks.map(t => (
        <TaskListItem
            key={`today-${t.id}`}
            task={t}
        />
    ));

    const completedTasks = taskStore.completedTasks.filter(
        t => t.folderId === folderId
    );

    const completedTaskComponents = completedTasks.map(t => (
        <TaskListItem
            key={`today-${t.id}`}
            task={t}
        />
    ));

    return (
        <>
            <strong>{folder.title}</strong>
            <Divider />
            {incompletedTaskComponents}
            <Divider />
            {completedTaskComponents}
        </>
    );
});

export default FolderTaskList;
