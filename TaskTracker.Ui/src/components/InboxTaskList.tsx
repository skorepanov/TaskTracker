import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../stores/RootStore";
import TaskListItem from "./TaskListItem";
import NoTasks from "./NoTasks";

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

export default InboxTaskList;
