import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import TaskList from "./TaskList";
import TaskCreationModalButton from "./TaskCreationModalButton";

const AllTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTasks = taskStore.getAllIncompletedTasks();
    const completedTasks = taskStore.getAllCompletedTasks();

    return (
        <>
            <div style={{ float: "left" }}>
                <strong>Все задачи</strong>
            </div>
            <div style={{ float: "right" }}>
                <TaskCreationModalButton />
            </div>
            <Divider />
            <TaskList
                incompletedTasks={incompletedTasks}
                completedTasks={completedTasks}
                shouldShowFolder={true}
            />
        </>
    );
});

export default AllTaskList;
