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
            <div style={{ float: "left", paddingLeft: 10, paddingTop: 10 }}>
                <strong>Все задачи</strong>
            </div>
            <div
                style={{ float: "right", paddingRight: 10, paddingBottom: 10 }}>
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
