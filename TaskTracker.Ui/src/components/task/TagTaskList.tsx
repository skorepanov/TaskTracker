import React from "react";
import { useParams } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import TaskListItem from "./TaskListItem";
import TaskTag from "../tag/TaskTag";
import NoTasks from "./NoTasks";

const TagTaskList: React.FC = observer(() => {
    const { taskStore, folderStore, tagStore } = useStore();

    const params = useParams();
    const tagId = Number(params.id);

    const tag = tagStore.tags.find(f => f.id === tagId);

    if (!tag) {
        return <NoTasks />;
    }

    const incompletedTasks = taskStore.incompletedTasks.filter(t =>
        t.tagIds?.includes(tagId)
    );

    const incompletedTaskComponents = incompletedTasks.map(t => (
        <TaskListItem
            key={t.id}
            task={t}
            shouldShowFolder={true}
            folder={folderStore.folders.find(f => f.id === t.folderId)}
        />
    ));

    const completedTasks = taskStore.completedTasks.filter(t =>
        t.tagIds?.includes(tagId)
    );

    const completedTaskComponents = completedTasks.map(t => (
        <TaskListItem
            key={t.id}
            task={t}
            shouldShowFolder={true}
            folder={folderStore.folders.find(f => f.id === t.folderId)}
        />
    ));

    return (
        <>
            <TaskTag tag={tag} />
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

export default TagTaskList;
