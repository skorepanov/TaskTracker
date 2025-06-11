import React from "react";
import { useParams } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/RootStore";
import TaskList from "./TaskList";
import NoTasks from "./NoTasks";
import TaskCreationPanel from "./TaskCreationPanel";

const FolderTaskList: React.FC = observer(() => {
    const { taskStore, folderStore } = useStore();

    const params = useParams();
    const folderId = Number(params.id);

    const folder = folderStore.folders.find(f => f.id === folderId);

    if (!folder) {
        return <NoTasks />;
    }

    const incompletedTasks = taskStore.getFolderIncompletedTasks(folderId);
    const completedTasks = taskStore.getFolderCompletedTasks(folderId);

    return (
        <>
            <div className="task-list-header">
                <strong>{folder.title}</strong>
                <TaskCreationPanel
                    key={`createTaskInFolder${folder.id}`}
                    folderId={folder.id}
                />
            </div>
            <TaskList
                incompletedTasks={incompletedTasks}
                completedTasks={completedTasks}
            />
        </>
    );
});

export default FolderTaskList;
