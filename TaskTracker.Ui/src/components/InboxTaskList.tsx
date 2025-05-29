import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../stores/RootStore";
import TaskListItem from "./TaskListItem";

const InboxTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTaskComponents = taskStore
        .getInboxIncompletedTasks()
        .map(t => (
            <TaskListItem
                key={`inbox-${t.id}`}
                task={t}
            />
        ));

    const completedTaskComponents = taskStore
        .getInboxCompletedTasks()
        .map(t => (
            <TaskListItem
                key={`inbox-${t.id}`}
                task={t}
            />
        ));

    return (
        <>
            <strong>Inbox</strong>
            <Divider />
            {incompletedTaskComponents}
            <Divider />
            {completedTaskComponents}
        </>
    );
});

export default InboxTaskList;
